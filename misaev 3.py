# матрица задание №3
matrix = [
  [11, 12, 13],
  [21, 22, 23],
  [31, 32, 33],
  [41, 42, 43],
  [51, 52, 53]
]
rows_count = len(matrix)
for i in range(1, rows_count, 2):

  current_row = matrix[i]
  row_to_print = ' '.join(map(str, current_row))
  print(row_to_print)
