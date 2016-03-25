filename = 'spinebase.txt';
delimiterIn = ' ';
headerlinesIn = 0;
A = importdata(filename, delimiterIn, headerlinesIn);
SpineBaseX = str2double(A.textdata(:,1));
SpineBaseY = str2double(A.textdata(:,2));
SpineBaseZ = str2double(A.textdata(:,3));
x = 1:length(SpineBaseX);
subplot(3, 1, 1), plot(SpineBaseX), ylabel('Position');
subplot(3, 1, 2), plot(SpineBaseY), ylabel('Position');
subplot(3, 1, 3), plot(SpineBaseZ), ylabel('Position');
xlabel('Sample');