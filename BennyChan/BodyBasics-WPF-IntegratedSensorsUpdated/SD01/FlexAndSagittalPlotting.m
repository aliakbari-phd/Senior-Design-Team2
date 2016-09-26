delimiterIn = ' ';
headerlinesIn = 0;

filenameA = 'flexAndSagittalAngleT2.txt';
A = importdata(filenameA, delimiterIn, headerlinesIn);

sagittalAngle = A(:,1);
sagittalAngle = A(:,1)-75;
flexAngle = A(:,2);
flexAngle = A(:,2)-15;

x_filter = designfilt('lowpassiir','FilterOrder',3,...
            'PassbandFrequency',10e3,'PassbandRipple',0.5,...
            'SampleRate',200e3);
sagittalAngle = filter(x_filter,sagittalAngle);
flexAngle = filter(x_filter,flexAngle);

t = A(:,3);
TestDuration = t-t(1);
TestDuration = TestDuration./1000;

set(gcf, 'color', 'white');
subplot(2, 1, 1), plot(TestDuration, flexAngle), ylabel('degrees');
title('Flex Angle');
ylim([30 120]);
subplot(2, 1, 2), plot(TestDuration, sagittalAngle), ylabel('degrees');
title('Sagittal Angle');
xlabel('seconds');
ylim([-45 45]);