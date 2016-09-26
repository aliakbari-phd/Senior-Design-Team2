filename = 'Ben_Johnston Cal 01.csv';
V_Data = xlsread(filename, 'A12:N1582');

pnts_base(:,1) = V_Data(:,3);           %base points
pnts_base(:,2) = V_Data(:,4);
pnts_base(:,3) = V_Data(:,5);

pnts_upper(:,1) = V_Data(:,9);          %upper points
pnts_upper(:,2) = V_Data(:,10);
pnts_upper(:,3) = V_Data(:,11);

v_zunit = ([0 0 1]);            %create z unit vector


v_pntpnt = pnts_upper - pnts_base;      %point to point vector

n=1;
i=1;
v_length = size(v_pntpnt);
l = v_length(1,1);
while i<l
v_pnt_norm(i,:) = v_pntpnt(i,:)./norm(v_pntpnt(i,:));
i = i+1;
theta(i,:) = acos(dot(v_pnt_norm(i,:),v_zunit));
end
alpha = (pi/2)-theta;
alpha_deg = alpha.*(180/pi);


% theta = atan2(norm(cross(v_pntpnt, v_zunit)), dot(v_pntpnt, v_zunit));

plot(V_Data(:,1),alpha_deg)

result = norm(v_pntpnt);

% hold on
% plot(V_Data(:,1),V_Data(:,11))
% hold on
% plot(V_Data(:,1),V_Data(:,5))
% hold on
% plot(V_Data(:,1),V_Data(:,4))
% hold on
% plot(V_Data(:,1),V_Data(:,10))
% hold on
% plot(V_Data(:,1),V_Data(:,3))
% hold on
% plot(V_Data(:,1),V_Data(:,9))