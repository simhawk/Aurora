clear all;
clc;

a = arduino('com5', 'uno');

left = [0, 0];
right = [34,0]./10./2.54;
top = [17, sqrt(34^2-17^2)]./10./2.54;

xdata = [left(1), right(1), top(1),0.5];
ydata = [left(2), right(2), top(2),0.5]
h = plot(xdata,ydata,'ro');

while true
    gaussLeft = readVoltage(a,'A0')/1.97*400;
    gaussTop = readVoltage(a,'A1')/1.97*400;
    gaussRight = readVoltage(a,'A2')/1.97*400;


    %quartic approximation coefficients
    c = 1.0e+03 *[ 0.2954 -3.5652 9.7244 -8.6980 2.6161];

    %1, 2, 3 represent left, right, then top respectively
    B = [gaussLeft; gaussRight; gaussTop];

    x0 = 13/16;

    fun1 = @(x)c(1)*x^4+c(2)*x^3+c(3)*x^2+c(4)*x+c(5) - B(1);
    fun2 = @(x)c(1)*x^4+c(2)*x^3+c(3)*x^2+c(4)*x+c(5) - B(2);
    fun3 = @(x)c(1)*x^4+c(2)*x^3+c(3)*x^2+c(4)*x+c(5) - B(3);

    R1 = fsolve(fun1, x0);
    R2 = fsolve(fun2, x0);
    R3 = fsolve(fun3, x0);

    p_guess = [13/16, 13/16];
    f = @(p) (sqrt((p(1)-left(1)).^2+(p(2)-left(2)).^2)-R1).^2   +   (sqrt((p(1)-right(1)).^2+(p(2)-right(2)).^2)-R2).^2   +   (sqrt((p(1)-top(1)).^2+(p(2)-top(2)).^2)-R3).^2;
    p_min = fminsearch(f,p_guess);
    
    
    h.XData = [left(1), right(1), top(1),p_min(1)];
    h.YData = [left(2), right(2), top(2),p_min(2)];
    pause(.1);
end