[200~numbers = [123, 45, 6789, 0, 100500]
for num in numbers:

    if num > 0:

        num_str = str(num)
        count = len(num_str)


        total = 0
        for digit in num_str:
            total += int(digit)

        print("Число:", num)
        print("Цифр:", count)
        print("Сумма цифр:", total)
        print()
