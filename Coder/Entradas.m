%% Coder
clc;
clear all;

datos = [1 0 1 0];

exportar_1 = double([(0:1e-3)', datos(1)'])
exportar_2 = double([(0:1e-3)', datos(2)'])
exportar_3 = double([(0:1e-3)', datos(3)'])
exportar_4 = double([(0:1e-3)', datos(4)'])

%% Decoder
clc;
clear all;

% 1 1 1 1 1 1 1 1
recibida = [0 0 1 0 0 0 1 0];

r_1 = double([(0:1e-3)', recibida(1)']);
r_2 = double([(0:1e-3)', recibida(2)']);
r_3 = double([(0:1e-3)', recibida(3)']);
r_4 = double([(0:1e-3)', recibida(4)']);
r_5 = double([(0:1e-3)', recibida(5)']);
r_6 = double([(0:1e-3)', recibida(6)']);
r_7 = double([(0:1e-3)', recibida(7)']);
r_8 = double([(0:1e-3)', recibida(8)']);

% Errores doble 5,6 y 7,8