clear all;
close all;
clc;    

I = eye(11); % Identidad
paridad=[1 0 0 1; % Paridad
    1 1 1 1;
    0 0 1 1;
    1 0 1 1;
    0 1 0 1;
    1 1 0 0;
    0 1 1 0;
    1 1 1 0;
    0 1 1 1;
    1 1 0 1;
    1 0 1 0];
g=[I paridad];
k=11;
for i=1:2^k 
    for j=k:-1:1
        if rem(i-1,2^(-j+k+1))>=2^(-j+k)
            u(i,j)=1;
        else
            u(i,j)=0;
        end
    end
end
c=rem(u*g,2);
d_min=min(sum((c(2:2^k,:))'));
