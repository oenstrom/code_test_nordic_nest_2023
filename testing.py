data = [
    {
        "validFrom": "1970-01-01T00:00:00",
        "validUntil": None,
        "unitPrice": 439.6
    },
    {
        "validFrom": "2018-06-18T00:00:00",
        "validUntil": "2018-08-05T00:00:00",
        # "validUntil": "2018-08-27T00:00:00",
        "unitPrice": 399.6
        # "unitPrice": 599.6
    },
    {
        "validFrom": "2018-08-01T00:00:00",
        "validUntil": "2018-08-05T00:00:00",
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


# Sort on  unitPrice low to high then on validUntil highest to lowest
lowest_price_data = sorted(data, key=lambda x: (-x["unitPrice"], x["validUntil"]), reverse=True)

# Sort the data on validFrom, validUntil, unitPrice
sorted_data = sorted(data, key=lambda x: (x["validFrom"], x["validUntil"] if x["validUntil"] else "9999-12-31T00:00:00", -x["unitPrice"]))
# Set all None values to 9999-12-31T00:00:00
for d in sorted_data:
    if d["validUntil"] is None:
        d["validUntil"] = "9999-12-31T00:00:00"


optimized_price = []
# Add the first entry from sorted_data to optimized_price
optimized_price.append({
    "validFrom": sorted_data[0]["validFrom"],
    "validUntil": sorted_data[0]["validUntil"],
    "unitPrice": sorted_data[0]["unitPrice"]
})

done = False
previous = sorted_data.pop(0)
while sorted_data:
    current = sorted_data.pop(0)

    # If there is a gap between the current price and the previous price
    if current["validFrom"] > previous["validUntil"]:
        # Add the lowest price that fits in the gap
        for entry in lowest_price_data:
            if entry["validFrom"] <= previous["validUntil"] and entry["validUntil"] > previous["validUntil"]:
                temp = { "validFrom": previous["validUntil"], "validUntil": entry["validUntil"], "unitPrice": entry["unitPrice"] }
                optimized_price.append(temp)
                previous = temp
                sorted_data.insert(0, current)
                break
        # No price fits in the gap, add the current price to the optimized_price
        else:
            optimized_price.append({ "validFrom": current["validFrom"], "validUntil": current["validUntil"], "unitPrice": current["unitPrice"] })
            previous = current
        continue

    
    # If the current price is lower than the previous price
    elif current["unitPrice"] < previous["unitPrice"]:
        optimized_price[-1]["validUntil"] = current["validFrom"]
        optimized_price.append({ "validFrom": current["validFrom"], "validUntil": current["validUntil"], "unitPrice": current["unitPrice"] })
    
    # Handle the remainders
    elif done:
        optimized_price.append({ "validFrom": previous["validUntil"], "validUntil": current["validUntil"], "unitPrice": current["unitPrice"] })
    
    previous = current
    if not sorted_data:
        sorted_data = [d for d in lowest_price_data if d["validUntil"] > optimized_price[-1]["validUntil"]]
        done = True



############################################################################################################
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