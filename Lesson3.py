# if 5 == 5: если равно 5
# if 5 <= 5: если меньше или равно
# if 5 >= 5: если больше или равно
# if 5 != 5: каждый раз когда не равно 5 в данном случаи
#     print("Yes")
# gad = int(input("Ведите число:"))
# look= False
# if  look: -- основное условие
#     print('your like my project')
# elif  gad == 5: -- дополнительное условие
#     print("Yes")
# else: -- сообщение о том что два условия является не верными
#     print('No')
# gad = int(input("Ведите число:"))
# look= False
# if not look and gad==5:
#     print('your like my project')
# else:
#     print('No')
#----------------------------------------------------------------------------------------------------------

# user=input()
# if user =="F":
#     print(5)
# else:
#     print(0,1)
# so=input()
# go = 5  if so == "F"   else 0
# print(go)
# User=input()
# read=True
# rea= 8 if User == '989' or  read==True else 0
# print(rea)
# if gad<5:
#     print('Число меньше 5!')
# if gad==5:
#     print('Good job')

# обрез строки
# value = "Hexlet"
# begin = 1
# end = 5
# value[begin:end] # вывод 'exle'
# value = "Hexlet"
# value[::] = "Hexlet"  # Вся строка
# value[:] = "Hexlet"  # Вся строка
# value[::2] = "Hxe"  # Нечетные по порядку символы
# value[1::2] = "elt"  # Четные по порядку символы
# value[::-1] = "telxeH"  # Вся строка в обратном порядке
# value[5:] = "t"  # Строка, начиная с шестого символа
# value[:5] = "Hexle"  # Строка до шестого символа
# value[-2:1:-1] = (
#     "elx"  # Все символы с предпоследнего до третьего в обратном порядке. Во всех случаях выборки от большего индекса к меньшему нужно указывать шаг
# )