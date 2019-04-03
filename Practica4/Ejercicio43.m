clear all;
close all;
clc;    

G=[1 0 0 0 1 1 0;
    0 1 0 0 0 1 1;
    0 0 1 0 1 1 1;
    0 0 0 1 1 0 1];

msg=[1 1 0 1];
code=encode(msg,7,4,'linear',G)
dec=decode(code,7,4,'linear',G)
%%

I = eye(11);
paridad=[1 0 0 1;
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
msgnew1=[1 1 0 0];
codenew=encode(msgnew1,15,11,'linear',g)
ms=decode(codenew, 15, 11, 'linear', g)


msgnew2=[1 0 0 0];
codenew2=encode(msgnew2,15,11,'linear',g)
ms1=decode(codenew2, 15, 11, 'linear', g)

