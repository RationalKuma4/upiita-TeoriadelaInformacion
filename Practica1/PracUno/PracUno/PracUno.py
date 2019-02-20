from time import time
import random
import re
import math

documento = open('mcil.txt','r')
texto = documento.read()
documento.close()
texto = texto.lower()
re.sub('[^a-zA-Z ]+?', '', texto)
longitud = len(texto)

diccionario= {} #declaramos diccionario vacio
for indice2 in range(0,longitud):#for donde veremos el caracter con el indice en el que se encuentra el for en el strng texto
    valor = texto[indice2]#guardamos el valor de ese indice, en este caso es la letra
    sumador = diccionario.get(valor,0)#sacamos el valor que tenga este caracter almacenado en nuestro diccionario, si no existe devuelve cero
    sumador=sumador+1#sumamos 1 a ese valor
    diccionario[valor]=sumador#actualizamos el valor de ese caracter

#sacamos el numero total de caracteres texto
sumatoriaDcicionario = sum(diccionario.values())#sumamos los valores de la aparicion de los caracteres, podriamos haber pueste el numero total de caracteres del texto pero se pone en caso de no saberlo y no contar con editor de texxto
#sacamos probabilidad de los caracteres que aparecen


probDicconario = {} #diccionrio vacioi para la probabilidad de cada caracter
InfoDiccionario = {}#diccionario vacio para informacion de cada caracter
entropiaDiccionario = {}#diccionario vacio para entropia de cada caracter
for clave,valor in diccionario.items():#for donde tenemos dos valores que recorrer, key y cale, estos valos salen de las tuplas que forma nuestro diccionario con la funcion items
    numero = diccionario.get(clave,0)#saamos el numero que tenga el caracter "clave", si no existe devuelve cero
    prob = numero/sumatoriaDcicionario #sacamos probabilidad del caracter
    probDicconario[clave] = prob##agregamos probabilidad de cada caracter
    info = math.log(1/prob,2)#sacamos la info del caracter en cuestion
    InfoDiccionario[clave] = info #agregamosa a diccionario de informacion la info de cada caracter
    entropiaDiccionario[clave] =  prob*info#agregamos entropia individual de cada caracyer

entropiaIndividuales = sum(entropiaDiccionario.values())#entropia dle texto como fuente sin memoria
#print(probDicconario.values())


#Lind = list(diccionario.items())
#Lind2 = sorted(Lind,key=lambda x:x[1],reverse=True)

#for listita in range(0,10):
#    print("aparicion descendente de cada caracter",Lind2[listita])
#######################################################################################################################################3
########################################################################################################################################

#sacamos repeticiones de pares
pares= {}
for indice2 in range(0,longitud-1):
    valor = texto[indice2:(indice2)+2]
    sumador = pares.get(valor,0)
    sumador=sumador+1
    pares[valor]=sumador


probPares = {}
infoPares = {}
entropiaPares = {}
listPares = list(pares.keys())
for indexad in range(0,len(listPares)-1):
    cadenaCompleta = listPares[indexad]#sacamos la palabra que aun esta en par
    indiceCedaPartida = len(cadenaCompleta)#sacamos la longitud de la palabra, siempre sera 3 pero por si no se sabe
    #fracCadena = cadenaCompleta[indiceCedaPartida-1]#sacamos la ultima letra de la palabra/cadena
    palabraRecortada = cadenaCompleta[0:indiceCedaPartida-1]#patimos la palabra, quitando la ultima letra
    #sabemos que la probabilidad es el numero de veces de la cadena completa sobre el numero de veces de esa palabra sin su ultimo caracter todo eso por probabilidad de la palabra recortada
    numeradorPares = pares.get(cadenaCompleta,0)#sacamos el numero de repeticiones de la cadena completa que es el numerador
    deonminadorPares = diccionario.get(palabraRecortada,0)#sacamos el numero de repeticiones de la palabra sin su ultimo caracter
    probRecortada = probDicconario.get(palabraRecortada)#sacamos la probabilidad de la palabra sin su ultimo caracter que es el del diccionario ed probabilidades anterior
    probCondicional = numeradorPares/deonminadorPares#sacamos la probabilidad condicional
    probCadenaCompleta = probCondicional*probRecortada#la probabilidad de esa cadena es entonces la multiplicacion de la proabilidad condicinoal por la probabilidad de la recortada
    probPares[cadenaCompleta] = probCadenaCompleta#asignamos ese valor al diccionario
    infroCadena = math.log(1/probCondicional,2)
    infoPares[cadenaCompleta] = infroCadena
    entropiaPares[cadenaCompleta] = infroCadena*probCadenaCompleta

entropiadePares = sum(entropiaPares.values())#entopia de cadena de markov de primer orden
#print(probPares.values())


#Lpar = list(pares.items())
#Lpar = sorted(Lpar,key=lambda x:x[1],reverse=True)
#for listita in range(0,10):
#    print("aparicion descendente de pares caracteres",Lpar[listita])
#######################################################################################################################################
#######################################################################################################################################
#sacamos repeticiones de tercias
tercias= {} #declaramos diccionario vacio
for indice2 in range(0,longitud-2):
    valor = texto[indice2:(indice2)+3]
    sumador = tercias.get(valor,0)
    sumador=sumador+1
    tercias[valor]=sumador


probTercias = {}
infoTercias = {}
entropiaTercias = {}
lisTercias = list(tercias.keys())
for indexad in range(0,len(lisTercias)-1):
    cadenaCompleta = lisTercias[indexad]#sacamos la palabra que aun esta en par
    indiceCedaPartida = len(cadenaCompleta)#sacamos la longitud de la palabra, siempre sera 3 pero por si no se sabe
    #fracCadena = cadenaCompleta[indiceCedaPartida-1]#sacamos la ultima letra de la palabra/cadena
    palabraRecortada = cadenaCompleta[0:indiceCedaPartida-1]#patimos la palabra, quitando la ultima letra
    #sabemos que la probabilidad es el numero de veces de la cadena completa sobre el numero de veces de esa palabra sin su ultimo caracter todo eso por probabilidad de la palabra recortada
    numeradorTercias = tercias.get(cadenaCompleta,0)#sacamos el numero de repeticiones de la cadena completa que es el numerador
    deonminadorTercias = pares.get(palabraRecortada,0)#sacamos el numero de repeticiones de la palabra sin su ultimo caracter
    probRecortada = probPares.get(palabraRecortada,0)#sacamos la probabilidad de la palabra sin su ultimo caracter que es el del diccionario ed probabilidades anterior
    probCondicional = numeradorTercias/deonminadorTercias#sacamos la probabilidad condicional
    probCadenaCompleta = probCondicional*probRecortada#la probabilidad de esa cadena es entonces la multiplicacion de la proabilidad condicinoal por la probabilidad de la recortada
    probTercias[cadenaCompleta] = probCadenaCompleta#asignamos ese valor al diccionario
    infroCadena = math.log(1/probCondicional,2)
    infoTercias[cadenaCompleta] = infroCadena
    entropiaTercias[cadenaCompleta] = infroCadena*probCadenaCompleta

entropiadeTercias = sum(entropiaTercias.values())#entropia de cadena de markov de segundo orden
#print(probTercias.values())

#Lter = list(tercias.items())
#Lter = sorted(Lter,key=lambda x:x[1],reverse=True)
#for listita in range(0,10):
#    print("aparicion descendente de taercias caracteres",Lter[listita])
######################################################################################################################################
#######################################################################################################################################
#sacamos repeticinoes de 4 caracteres seguidos
cuarteto= {} #declaramos diccionario vacio
for indice2 in range(0,longitud-3):
    valor = texto[indice2:(indice2)+4]
    sumador = cuarteto.get(valor,0)
    sumador=sumador+1
    cuarteto[valor]=sumador


listaux = list(cuarteto.keys())
for numero in range(0,len(cuarteto)):
    clave = listaux[numero]
    if '\n' in clave:
        del cuarteto[clave]
    elif '\xa0' in clave:
        del cuarteto[clave]
    elif '·' in clave:
        del cuarteto[clave]
    elif 'Â' in clave:
        del cuarteto[clave]
    elif '\x00' in clave:
        del cuarteto[clave]
    elif '/' in clave:
        del cuarteto[clave]
    elif 'þ' in clave:
        del cuarteto[clave]
    elif 'ï' in clave:
        del cuarteto[clave]
    elif '§' in clave:
        del cuarteto[clave]

probCuarteto = {}
infoCuarteto = {}
entropiaCuarteto = {}
lisCuarteto = list(cuarteto.keys())
for indexad in range(0,len(lisCuarteto)-1):
    cadenaCompleta = lisCuarteto[indexad]#sacamos la palabra que aun esta en par
    indiceCedaPartida = len(cadenaCompleta)#sacamos la longitud de la palabra, siempre sera 3 pero por si no se sabe
    #fracCadena = cadenaCompleta[indiceCedaPartida-1]#sacamos la ultima letra de la palabra/cadena
    palabraRecortada = cadenaCompleta[0:indiceCedaPartida-1]#patimos la palabra, quitando la ultima letra
    #sabemos que la probabilidad es el numero de veces de la cadena completa sobre el numero de veces de esa palabra sin su ultimo caracter todo eso por probabilidad de la palabra recortada
    numeradorCuarteto = cuarteto.get(cadenaCompleta,0)#sacamos el numero de repeticiones de la cadena completa que es el numerador
    deonminadorCuarteto = tercias.get(palabraRecortada,0)#sacamos el numero de repeticiones de la palabra sin su ultimo caracter
    probRecortada = probTercias.get(palabraRecortada,0)#sacamos la probabilidad de la palabra sin su ultimo caracter que es el del diccionario ed probabilidades anterior
    probCondicional = numeradorCuarteto/deonminadorCuarteto#sacamos la probabilidad condicional
    probCadenaCompleta = probCondicional*probRecortada#la probabilidad de esa cadena es entonces la multiplicacion de la proabilidad condicinoal por la probabilidad de la recortada
    probCuarteto[cadenaCompleta] = probCadenaCompleta#asignamos ese valor al diccionario
    infroCadena = math.log(1/probCondicional,2)
    infoCuarteto[cadenaCompleta] = infroCadena
    entropiaCuarteto[cadenaCompleta] = infroCadena*probCadenaCompleta

entropiadeCuarteto = sum(entropiaCuarteto.values())#entropia de cadena de markov de segundo orden

#Lcuar = list(cuarteto.items())
#Lcuar = sorted(Lcuar,key=lambda x:x[1],reverse=True)
#for listita in range(0,10):
#    print("aparicion descendente de cuartetos de  caracteres",Lcuar[listita])
###########################################################################################################################################
##########################################################################################################################################3
#sacamos repeticiones de 5 caracteres seguidos
quinteto= {} #declaramos diccionario vacio
for indice2 in range(0,longitud-4):
    valor = texto[indice2:(indice2)+5]
    sumador = quinteto.get(valor,0)
    sumador=sumador+1
    quinteto[valor]=sumador

listaux = list(quinteto.keys())
for numero in range(0,len(quinteto)):
    clave = listaux[numero]
    if '\n' in clave:
        del quinteto[clave]
    elif '\xa0' in clave:
        del quinteto[clave]
    elif '·' in clave:
        del quinteto[clave]
    elif 'Â' in clave:
        del quinteto[clave]
    elif '\x00' in clave:
        del quinteto[clave]
    elif '/' in clave:
        del quinteto[clave]
    elif 'þ' in clave:
        del quinteto[clave]
    elif 'ï' in clave:
        del quinteto[clave]
    elif '§' in clave:
        del quinteto[clave]

probQuinteto = {}
infoQuinteto = {}
entropiaQuinteto = {}
lisQuinteto = list(quinteto.keys())
for indexad in range(0,len(lisQuinteto)-1):
    cadenaCompleta = lisQuinteto[indexad]#sacamos la palabra que aun esta en par
    indiceCedaPartida = len(cadenaCompleta)#sacamos la longitud de la palabra, siempre sera 3 pero por si no se sabe
    #fracCadena = cadenaCompleta[indiceCedaPartida-1]#sacamos la ultima letra de la palabra/cadena
    palabraRecortada = cadenaCompleta[0:indiceCedaPartida-1]#patimos la palabra, quitando la ultima letra
    #sabemos que la probabilidad es el numero de veces de la cadena completa sobre el numero de veces de esa palabra sin su ultimo caracter todo eso por probabilidad de la palabra recortada
    numeradorQuinteto = quinteto.get(cadenaCompleta,0)#sacamos el numero de repeticiones de la cadena completa que es el numerador
    deonminadorQuinteto = cuarteto.get(palabraRecortada,0)#sacamos el numero de repeticiones de la palabra sin su ultimo caracter
    probRecortada = probCuarteto.get(palabraRecortada,0)#sacamos la probabilidad de la palabra sin su ultimo caracter que es el del diccionario ed probabilidades anterior
    probCondicional = numeradorQuinteto/deonminadorQuinteto#sacamos la probabilidad condicional
    probCadenaCompleta = probCondicional*probRecortada#la probabilidad de esa cadena es entonces la multiplicacion de la proabilidad condicinoal por la probabilidad de la recortada
    probQuinteto[cadenaCompleta] = probCadenaCompleta#asignamos ese valor al diccionario
    infroCadena = math.log(1/probCondicional,2)
    infoQuinteto[cadenaCompleta] = infroCadena
    entropiaQuinteto[cadenaCompleta] = infroCadena*probCadenaCompleta

entropiadeQuinteto = sum(entropiaQuinteto.values())#entropia de cadena de markov de segundo orden


#Lquin = list(quinteto.items())
#Lquin = sorted(Lquin,key=lambda x:x[1],reverse=True)
#for listita in range(0,10):
#    print("aparicion descendente de quintetos de  caracteres",Lquin[listita])

#Lquin = list(probQuinteto.items())
#Lquin = sorted(Lquin,key=lambda x:x[1],reverse=True)
#for listita in range(0,10):
#    print("probabilidad de cuarta ccadena de markov por cada quinteto de caracteres",Lquin[listita])


#Lquininf = list(infoQuinteto.items())
#Lquininf = sorted(Lquin,key=lambda x:x[1],reverse=True)
#for listita in range(0,len(Lquininf)):
#    print("informacion de cuarta ccadena de markov por cada quinteto de caracteres",Lquininf[listita])



#Li1 = list(probDicconario.keys())
indiceCdena = 0;

while indiceCdena < 2000:
    palabra = random.choice(list(probDicconario.keys()))
    #creamos listas auxiliares donde buscaremos las combinaciones que tienen la letra que tenemos
    word = list()
    AuxPar = {}
    AuxTer = {}
    AuxCuar = {}
    AuxQui = {}

    for indice1 in probPares:
        if palabra in indice1[0:len(indice1)-1]:
            AuxPar[indice1]=probPares[indice1]
    for sub in AuxPar:
           if AuxPar[sub] == max(AuxPar.values()):
               palabra = sub
    for indice in probTercias:
        if palabra in indice[0:len(indice)-1]:
            AuxTer[indice] = probTercias[indice]
    for sub in AuxTer:
           if AuxTer[sub] == max(AuxTer.values()):
               palabra = sub
    for indice in probCuarteto:
       if palabra in indice[0:len(indice)-1]:
           AuxCuar[indice] = probCuarteto[indice]
    for sub in AuxCuar:
           if AuxCuar[sub] == max(AuxCuar.values()):
               palabra = sub
    for indice in probQuinteto:
        if palabra in indice[0:len(indice)-1]:
            AuxQui[indice] = probQuinteto[indice]
    for sub in AuxQui:
           if AuxQui[sub] == max(AuxQui.values()):
               palabra = sub
    word.append(palabra)
    indiceCdena=indiceCdena+5
    #palabra = palabra[len(palabra)-1]
    del(AuxPar)
    del(AuxTer)
    del(AuxCuar)
    del(AuxQui)
    #3if palabra==' ':
    #    print("".join(word),end="")
    #elif palabra != ' ':
    #    print("".join(word),end="")
    print("".join(word),end="")
    del(word)
#print("".join(word))



#print("la entropia como fuente sin memoria es ",entropiaIndividuales)
#print("la entropia como markov primer orden ",entropiadePares)
#print("la entropia como markov segundo orden",entropiadeTercias)
#print("la entropia como markov tercer orden",entropiadeCuarteto)
#print("la entropia como markov cuarto orden",entropiadeQuinteto)
tiempo_final = time()
tiempo_ejecucion = tiempo_final - tiempo_inicial
print("la duracion de ejecucion del programa es de ",tiempo_ejecucion)
#
