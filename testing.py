data = [
    {
        "validFrom": "1970-01-01T00:00:00",
        "validUntil": None,
        "unitPrice": 439.6
    },
    {
        "validFrom": "2018-06-18T00:00:00",
        "validUntil": "2018-08-05T00:00:00",
        "unitPrice": 399.6
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

# Sortera på validFrom, plocka ut alla som har validUntil = None, lägg till dem i en lista
null_prices = [d for d in sorted(data, key=lambda x: x["validFrom"]) if d["validUntil"] is None]

# Sätt last_valid_until till lägsta validFrom
last_valid_until = "1970-01-01T00:00:00"
# Plocka ut första default_price
default_price = null_prices[0]["unitPrice"]

# Sortera datan på validFrom, validUntil, unitPrice
sorted_data = sorted(data, key=lambda x: (x["validFrom"], x["validUntil"] if x["validUntil"] else "9999-12-31T00:00:00", -x["unitPrice"]))

# Loopa igenom datan
# for i, d in enumerate(sorted_data):


# print("Price", "Valid From", "\tValid Until", sep="\t")
# for d in sorted(optimized_price, key=lambda x: x["validFrom"]):
#     print(d["unitPrice"], d["validFrom"], d["validUntil"], sep="\t")

# print("-----------------------------------------------------")

print("Price", "Valid From", "\tValid Until", sep="\t")
for d in data:
    print(d["unitPrice"], d["validFrom"], d["validUntil"], sep="\t")

print("-----------------------------------------------------")

print("Price", "Valid From", "\tValid Until", sep="\t")
for d in expected_output:
    print(d["unitPrice"], d["validFrom"], d["validUntil"], sep="\t")
