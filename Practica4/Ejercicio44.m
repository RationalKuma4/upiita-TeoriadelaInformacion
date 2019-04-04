close all;
clear all;
clc;

pb157=zeros(1,50);
pb74=zeros(1,50);
pb1511=zeros(1,50);
p=0.07864:-0.0015:0.0059-0.0015;
j=1;

for i=p
    % BCH(15,7)
    pbsc157=i;
    simOut157 = sim('codec157.slx');
    pb157(j)=pbModel157(1);
    
    % BCH(7,4)
    pbsc74=i;
    simOut74 = sim('codec74.slx');
    pb74(j)=pbModel74(1);
    
    % BCH(15,11)
    pbsc1511=i;
    simOut1511 = sim('codec1511.slx');
    pb1511(j)=pbModel1511(1);
    
    j=j+1;
end

figure(1);
plot(p,pb157,'LineWidth',1.5);
title('$BCH(15,7)$','Interpreter','latex');
xlabel('$P$','Interpreter','latex');
ylabel('$Pb$','Interpreter','latex');
grid on;

figure(2);
plot(p,pb74,'LineWidth',1.5);
title('$BCH(7,4)$','Interpreter','latex');
xlabel('$P$','Interpreter','latex');
ylabel('$Pb$','Interpreter','latex');
grid on;

figure(3);
plot(p,pb1511,'LineWidth',1.5);
title('$BCH(15,11)$','Interpreter','latex');
xlabel('$P$','Interpreter','latex');
ylabel('$Pb$','Interpreter','latex');
grid on;

figure(4);
plot(p,pb157,p,pb74,p,pb1511,'LineWidth',1.5);
title('$BCH(15,11), \ BCH(7,4), \ BCH(15,11)$','Interpreter','latex');
xlabel('$P$','Interpreter','latex');
ylabel('$Pb$','Interpreter','latex');
legend('BCH(15,7)','BCH(7,4)','BCH(15,11)');
grid on;
