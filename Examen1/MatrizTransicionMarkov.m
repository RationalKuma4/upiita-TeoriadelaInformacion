clear all;
close all;
clc;

%%
matrizTransicion=[1/3 1/3 1/3; 1/2 0 1/2; 3/4 1/4 0];

for i=2:20
    matrizTransicion^i
end



H_c=.3*(1/3)*log2(3)+.3*(1/3)*log2(3)+.2*(1/3)*log2(3)+...
    .3*(1/2)*log2(2)+.2*(1/2)*log2(2)+.1*(1/4)*log2(4)+...
    .3*(3/4)*log2(4/3)

%%
symbols = [1 2 3];
prob = [.45 .35 .2];

dict = huffmandict(symbols,prob)

%%
clear all;
close all;

m=input('Enter the no. of message ensembles : ');
z=[];
h=0;l=0;
display('Enter the probabilities in descending order');
for i=1:m
    fprintf('Ensemble %d\n',i);
    p(i)=input('');
end
%Finding each alpha values 
a(1)=0;
for j=2:m;
    a(j)=a(j-1)+p(j-1);
end
fprintf('\n Alpha Matrix');
display(a);
%Finding each code length
for i=1:m
    n(i)= ceil(-1*(log2(p(i))));
end
fprintf('\n Code length matrix');
display(n);
%Computing each code
for i=1:m
    int=a(i);
for j=1:n(i)
    frac=int*2;
    c=floor(frac);
    frac=frac-c;
    z=[z c];
    int=frac;
end
fprintf('Codeword %d',i);
display(z);
z=[];
end
%Computing Avg. Code Length & Entropy
fprintf('Avg. Code Length');
for i=1:m
    x=p(i)*n(i);
    l=l+x;
    x=p(i)*log2(1/p(i));
    h=h+x;
end
display(l);
fprintf('Entropy');
display(h);
%Computing Efficiency
fprintf('Efficiency');
display(100*h/l);
fprintf('Redundancy');
display(100-(100*h/l));