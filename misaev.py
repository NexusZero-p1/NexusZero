X = int(input())
H = int(input())
M = int(input())

total = H * 60 + M + X
print(total // 60)
print(total % 60)