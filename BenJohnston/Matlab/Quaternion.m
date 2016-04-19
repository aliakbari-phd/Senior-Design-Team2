filename = '200 Hz 15 sec 01.txt';
delimiterIn = '\t';
headerlinesIn = 1;
A = importdata(filename, delimiterIn, headerlinesIn);
acceleration(:,1)=(A.data(:,1))./4096;
acceleration(:,2)=(A.data(:,2))./4096;
acceleration(:,3)=(A.data(:,3))./4096;
velocity(:,1)=(A.data(:,4))./(5895/pi);
velocity(:,2)=(A.data(:,5))./(5895/pi);
velocity(:,3)=(A.data(:,6))./(5895/pi);
t(1,:) = (A.data(:,13))./1000;

T = (t(end)-t(1))/length(velocity);


%{
delta_t = 0;
for n=2:length(t)
    delta_t(end+1,:)=t(n)-t(n-1);
end;
delta_t(:,2)=delta_t(:,1);
delta_t(:,3)=delta_t(:,1);

aa = velocity.*T;     %axis angle

for i=1:length(aa)
    angle(i,:,1) = sqrt(aa(i,1).^2+aa(i,2).^2+aa(i,3).^2);
end    

aa(:,1) = aa(:,1)./angle;   %normalize
aa(:,2) = aa(:,2)./angle;
aa(:,3) = aa(:,3)./angle;

q(:,1) = cos(angle./2);     %quaternion
q(:,2) = aa(:,1).*sin(angle./2);
q(:,3) = aa(:,2).*sin(angle./2);
q(:,4) = aa(:,3).*sin(angle./2);

check = q(:,1).^2 + q(:,2).^2 + q(:,3).^2 + q(:,4).^2;

%}