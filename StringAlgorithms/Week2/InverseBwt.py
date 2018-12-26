# python3
import sys


def inverse_bwt(text):
    lastColumn = [(value, index) for (index, value) in enumerate(text)]
    firstColumn = sorted(lastColumn)
    first_to_last = {first: last for first, last in zip(firstColumn, lastColumn)}

    next = firstColumn[0]
    result = ''
    for i in range(len(text)):
        result += next[0]
        next = first_to_last[next]

    return result[::-1]


if __name__ == '__main__':
    bwt = sys.stdin.readline().strip()
    print(inverse_bwt(bwt))
