clear all;
clc;

a = arduino('com5', 'uno');

plotTitle = 'Arduino Data Log';  % plot title
xLabel = 'Elapsed Time (s)';     % x-axis label
yLabel = 'Analog (G)';      % y-axis label
legend1 = 'Temperature Sensor 1'
legend2 = 'Temperature Sensor 2'
legend3 = 'Temperature Sensor 3'
yMax  = 800                           %y Maximum Value
yMin  = 0                       %y minimum Value
plotGrid = 'on';                 % 'off' to turn off grid
min = 0;                         % set x-min
max = 800;                        % set x-max
delay = .01;                     % make sure sample faster than resolution 
%Define Function Variables
time = 0;
data = 0;
data1 = 0;
data2 = 0;
count = 0;
%Set up Plot
plotGraph = plot(time,data,'-r' )  % every AnalogRead needs to be on its own Plotgraph
hold on                            %hold on makes sure all of the channels are plotted
plotGraph1 = plot(time,data1,'-b')
plotGraph2 = plot(time, data2,'-g' )
title(plotTitle,'FontSize',15);
xlabel(xLabel,'FontSize',15);
ylabel(yLabel,'FontSize',15);
legend(legend1,legend2,legend3)
axis([yMin yMax min max]);
grid(plotGrid);
tic
while ishandle(plotGraph) %Loop when Plot is Active will run until plot is closed
         dat = readVoltage(a,'A0')/1.99*400; %Data from the arduino
         dat1 = readVoltage(a,'A1')/1.99*400; 
         dat2 = readVoltage(a,'A2')/1.99*400;       
         count = count + 1;    
         time(count) = toc;    
         data(count) = dat(1);         
         data1(count) = dat1(1);
         data2(count) = dat2(1);
         %This is the magic code 
         %Using plot will slow down the sampling time.. At times to over 20
         %seconds per sample!
         set(plotGraph,'XData',time,'YData',data);
         set(plotGraph1,'XData',time,'YData',data1);
         set(plotGraph2,'XData',time,'YData',data2);
          axis([0 time(count) min max]);
          %Update the graph
          pause(delay);
  end



