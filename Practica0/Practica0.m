close all;
clear all;

%Abrir archivo
archivo=fopen('C:\Users\Anselmo-PC\Documents\GitHub\upiita-TeoriadelaInformacion\Practica0\principito2.txt','r');
cadena=fscanf(archivo,'%c');
numcaracteres=length(cadena);
fclose(archivo);

% Vector con 26 letra de 27 mas espacio, 27 simbolos en total
ABC=['a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r'...
,'s','t','u','v','w','x','y','z',' ','ñ'];

%conteo de reptyiciones por cada caracter
ABC0=zeros(1,length(ABC));
cont=1;

for x=1:length(ABC)
    rep=0; %variable donde se guarda el numero de repetuiciones por caracter
    for y=1:length(cadena)
        if cadena(y)==ABC(x)
            rep=rep+1;
        end
    end
    ABC0(cont)=rep;
    cont=cont+1;
    fprintf('\n%c repeticiones: %d',ABC(x),ABC0(x));
end

p1=zeros(1,length(ABC0));
l1=zeros(1,length(ABC0));

for x=1:length(ABC0)
    p=(ABC0(x)./sum(ABC0));
    p1(x)=p;
    
    l=log2(1/p);
    if l==inf
        l=0;
    end
    l1(x)=l;
    fprintf('\n%c:\trepeticiones %f\t\tP= %f\t\tl= %f bits \n',ABC(x),ABC0(x),p,l);
end

%calculo de la entropia como fuente sin memoria
h=zeros(1,length(p1));
for x=1:length(p1)
    h(x)=p1(x)*l1(x);
end

H=sum(h);
fprintf('\nEntropia dcom ofuente sin memoria= %f bits/sibl\n',H);

A=categorical({'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r'...
,'s','t','u','v','w','x','y','z','esp','ñ'});

figure(1)
subplot(1,2,1);
bar(A,p1);
xlabel('Simbiolos');
ylabel('Probabilidad');
subplot(1,2,2);
bar(A,l1);
xlabel('Simbiolos');
ylabel('Cantidad de informacion');

