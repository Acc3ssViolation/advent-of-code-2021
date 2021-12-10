bit_counts = []
lines = 0

with open('day03.txt') as f:
    while True:
        line = f.readline().strip()
        if line == '':
            break
        if len(bit_counts) == 0:
            for c in line:
                bit_counts.append(0)
        for i, c in enumerate(line):
            if c == '1':
                bit_counts[i] += 1
        lines += 1

for n in bit_counts:
    if n > lines / 2:
        print('1', end='')
    else:
        print('0', end='')
