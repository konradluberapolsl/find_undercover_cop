import os

char_set = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0']
files = os.listdir("./IMG")
f = os.listdir("./IMG/Segregated")
path = os.getcwd()


def menu():
    o = input("Co checsz zrobić\n1.Segreguj\n2.Szukaj\n")
    if o=="1":
        segregate()
    elif o=="2":
        f = input("Jaka rejestracje chcesz sprawdzić? \n")
        f+=".jpg"
        print(search(f))

def search(name):
    return name in files + f
    
def segregate():
        for f in files:
            if f != "Segregated":
                for char in char_set:
                    dir = '.\Training\\' + char
                    if not os.path.isdir(dir):
                        os.mkdir(dir)
                    tmp = dir + f
                    if f.rfind(char)!= -1 and not os.path.exists(tmp):
                        tmp1 = '.\IMG\\'+ f
                        tmp2 = 'copy "' + tmp1 +'" "'+dir+'\\'+f+'"'
                        print(tmp2)
                        os.system(tmp2)

menu()
input()



