import numpy
from PIL import Image

filePath = "slope.txt"
w, h = 382, 266
size = 513

data = numpy.zeros([size,size], dtype="int8")
for y, line in enumerate(open(filePath), start=int((size - h) / 2)):
    for x, value in enumerate([float(value) for value in line.split()], start=int((size - w) / 2)):
        data[y,x] = value / 64 * 255 if value > 0 else 0

img = Image.fromarray(data, mode='L').show()
