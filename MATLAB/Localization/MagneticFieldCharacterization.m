clear all;
clc;

A = [81.8 79; % rough data for radial field
    143.3,141;
    198.8 197;
    250.8 249;
    292.7 293;
    329 325;
    356.6 354;
    371 372;
    385 388;
    397 399];

R = (1/2 + (1/16:1/16:10/16)).'; % radial distance in inches
B = (A(:,1)+A(:,2))/2; % Average field intensity
plot(R, B, 'ro');
title('Magnetic Field');

xdata = R';
ydata = B';

t = (1/2+1/16):0.01:(1/2+10/16);

%%%%%%%%%%%%%%%%%%%%%%%%
%       Curve fitting
%%%%%%%%%%%%%%%%%%%%%%%%

% Quadratic
fun = @(x, xdata)(x(1)+x(2)*xdata+x(3)*xdata.^2);
x0 = [ 1, 1, 1];
[x,norm_quad] = lsqcurvefit(fun,x0, xdata, ydata);
R = x(1)+x(2)*t+x(3)*t.^2; 
hold on
%plot(t,R);
hold off

% Cubic
fun = @(x, xdata)(x(1)+x(2)*xdata+x(3)*xdata.^2+x(4)*xdata.^3);
x0 = [ 1, 1, 1,1];
[x_cubic,norm_cubic] = lsqcurvefit(fun,x0, xdata, ydata);
R = x_cubic(1)+x_cubic(2)*t+x_cubic(3)*t.^2+x_cubic(4)*t.^3; 
hold on
%plot(t,R);
hold off

% Quartic
fun = @(x, xdata)(x(1)+x(2)*xdata+x(3)*xdata.^2+x(4)*xdata.^3+x(5)*xdata.^4);
x0 = [ 1, 1, 1,1,1];
[x_quar,norm_quartic] = lsqcurvefit(fun,x0, xdata, ydata);
R = x_quar(1)+x_quar(2)*t+x_quar(3)*t.^2+x_quar(4)*t.^3+x_quar(5)*t.^4; 
hold on
plot(t,R);
hold off

% Exponential
fun = @(x,xdata)(x(1)-x(1)*exp(-(xdata-x(2))/x(3)));
x0 = [400, -1, 1];
[x_exp, norm_exp] = lsqcurvefit(fun, x0, xdata, ydata);
R = x_exp(1)-x_exp(1).*exp(-(t-x_exp(2))/x_exp(3));
hold on 
%plot(t,R);
hold off;

% Double Exponential, probs not a good fit
fun = @(x,xdata)x(1)*(1-exp(-(xdata-x(2))/x(3)))+x(4)*(1-exp(-(xdata-x(5))/x(6)));
x0 = [200, -1, 1,200,-2,3];
[x_exp2,norm_exp2] = lsqcurvefit(fun, x0, xdata, ydata);
R = x_exp2(1)*(1-exp(-(t-x_exp2(2))/x_exp2(3)))+x_exp2(4)*(1-exp(-(t-x_exp2(5))/x_exp2(6)));
hold on 
%plot(t,R);
hold off;


% guess if lim x-> inf is unknown
% x = 468.6342    0.5072    0.3151