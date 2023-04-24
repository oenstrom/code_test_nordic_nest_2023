data = [
    # {
    #     "validFrom": "1970-01-01T00:00:00",
    #     "validUntil": None,
    #     "unitPrice": 439.6
    # },
    {
        "validFrom": "2018-06-18T00:00:00",
        "validUntil": "2018-08-05T00:00:00",
        "unitPrice": 399.6
    },
    {
        "validFrom": "2018-08-01T00:00:00",
        # "validUntil": "2018-08-05T00:00:00",
        "validUntil": "2018-08-03T00:00:00",
        "unitPrice": 326.8
    },
    {
        "validFrom": "2018-08-07T00:00:00",
        "validUntil": "2018-08-19T00:00:00",
        "unitPrice": 326.8
    }
]

expected_output = [
    {
        "validFrom": "1970-01-01T00:00:00",
        "validUntil": "2018-06-18T00:00:00",
        "unitPrice": 439.6
    },
    {
        "validFrom": "2018-06-18T00:00:00",
        "validUntil": "2018-08-01T00:00:00",
        "unitPrice": 399.6
    },
    {
        "validFrom": "2018-08-01T00:00:00",
        "validUntil": "2018-08-05T00:00:00",
        "unitPrice": 326.8
    },
    {
        "validFrom": "2018-08-05T00:00:00",
        "validUntil": "2018-08-07T00:00:00",
        "unitPrice": 439.6
    },
    {
        "validFrom": "2018-08-07T00:00:00",
        "validUntil": "2018-08-19T00:00:00",
        "unitPrice": 326.8
    },
    {
        "validFrom": "2018-08-19T00:00:00",
        "validUntil": None,
        "unitPrice": 439.6
    },
]

optimized_price = []

# Sort on validFrom, get all entries with validUntil = None, add them to a list
null_prices = [d for d in sorted(data, key=lambda x: x["validFrom"]) if d["validUntil"] is None]

# Sort the data on validFrom, validUntil, unitPrice
sorted_data = sorted(data, key=lambda x: (x["validFrom"], x["validUntil"] if x["validUntil"] else "9999-12-31T00:00:00", -x["unitPrice"]))

# Add the first entry from sorted_data to optimized_price
optimized_price.append({
    "validFrom": sorted_data[0]["validFrom"],
    "validUntil": sorted_data[0]["validUntil"],
    "unitPrice": sorted_data[0]["unitPrice"]
})

# Loop through the sorted data
for i, entry in enumerate(sorted_data):
    if i == 0:
        continue
    
    # Om föregående validUntil är None eller större än nuvarande validFrom. Sätt in nuvarande validFrom som validUntil och lägg till nuvarande entry
    if optimized_price[i - 1]["validUntil"] is None or entry["validFrom"] <= optimized_price[i - 1]["validUntil"]:
        optimized_price[i - 1]["validUntil"] = entry["validFrom"]
        optimized_price.append({
            "validFrom": entry["validFrom"],
            "validUntil": entry["validUntil"],
            "unitPrice": entry["unitPrice"]
        })
    else:
        # Find lowest price in null_prices
        valid_null_prices = [d for d in null_prices if d["validFrom"] <= entry["validFrom"]]
        default = None if len(valid_null_prices) == 0 else min(valid_null_prices, key=lambda x: x["unitPrice"])["unitPrice"]
        if default is None:
            optimized_price.append({
                "validFrom": entry["validFrom"],
                "validUntil": entry["validUntil"],
                "unitPrice": entry["unitPrice"]
            })
            continue
        # Add default price
        optimized_price.append({
            "validFrom": optimized_price[i - 1]["validUntil"],
            "validUntil": entry["validFrom"],
            "unitPrice": default
        })
        # Add entry
        optimized_price.append({
            "validFrom": entry["validFrom"],
            "validUntil": entry["validUntil"],
            "unitPrice": entry["unitPrice"]
        })


# Find lowest price in null_prices
valid_null_prices = [d for d in null_prices if d["validFrom"] <= optimized_price[-1]["validUntil"]]
default = None if len(valid_null_prices) == 0 else min(valid_null_prices, key=lambda x: x["unitPrice"])["unitPrice"]
# Add last default price
if default is not None:
    optimized_price.append({
        "validFrom": optimized_price[-1]["validUntil"],
        "validUntil": None,
        "unitPrice": default
    })


print()
print("=====================================================")
print("___________________The data output___________________")
print("-----------------------------------------------------")
print("Price", "Valid From", "\tValid Until", sep="\t")
for d in data:
    print(d["unitPrice"], d["validFrom"], d["validUntil"], sep="\t")


print()
print("=====================================================")
print("___________________Optimized output__________________")
print("-----------------------------------------------------")
print("Price", "Valid From", "\tValid Until", sep="\t")
for d in sorted(optimized_price, key=lambda x: x["validFrom"]):
    print(d["unitPrice"], d["validFrom"], d["validUntil"], sep="\t")


print()
print("=====================================================")
print("___________________Expected output___________________")
print("-----------------------------------------------------")
print("Price", "Valid From", "\tValid Until", sep="\t")
for d in expected_output:
    print(d["unitPrice"], d["validFrom"], d["validUntil"], sep="\t")
