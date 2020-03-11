import numpy
import json

fuel = { "fuel" + str(i) : [] for i in range(14)}

for y, line in enumerate(open("fuel.txt")):
    for x, value in enumerate([int(value) for value in line.split() if 0 <= int(value) <= 13 ]):
        fuel["fuel" + str(value)].append([x,y])

with open('jsonFuel.txt', 'w') as f:
    f.write(json.dumps({
        "values":fuel
    }))
