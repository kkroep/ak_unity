clear;
clc;
close;

colorspec = {[0.4 0 0.8]; [0.4 0.8 0]; [0.4 0.7 0.7]; ...
  [0 0.4 0.8]; [0.8 0.4 0]; [0.7 0.4 0.7]; ...
  [0.8 0 0.4]; [0 0.8 0.4]; [0.7 0.7 0.4]; ...
  [0 0 0.7]; [0 0.7 0]; [0.7 0 0]};

colorspec = {...
[0.0 0 1.0]; ...
[0.2 0 0.8]; ... 
[0.4 0 0.6]; ... 
[0.6 0 0.4]; ... 
[0.8 0 0.2]; ... 
[1.0 0 0.0]; ... 
};

%graphics_toolkit gnuplot;
%figure ("visible", "off");

files = dir('*.csv');

C2=[];
ty = [];

legendstring = [];

hold on;

for i=size(files,1):-1:1
    C1 = csvread(files(i).name);
    %delay = str2num(files(i).name(2:6));
    %C1(1,:) = []; % clear out the text row
    %test = C1(C1(:,2)>delay-0.001, :);
    %data{i} = C1;
    %names(i,:) = files(i).name;
    plot(C1(:,1), C1(:,2));
    %legendstring{end+1} = [files(i).name(2:6) '%'];
end

%size(data)
%names


%for i=size(files,1):-1:46
    %plot(data{i}(:,1), (data{i}(:,2)));
%end

hold off;

%plot(C1(:,1),C1(:,4), 'Color', colorspec{mod(i,12)+1});
%axis([C1(1,1) C1(end,1) min(min(C1))*1.1 max(max(C1))*1.1]);
xlim([min(C1(:,1)), max(C1(:,1))]);
%ylim([1e-1, 1e2]);
%xlabel('packetrate [s]', 'fontsize', 14);
%ylabel('RMSE [-]', 'fontsize', 14);
%set(gca, 'FontSize', 12)


%legend(legendstring, 'Location', 'northeastoutside');
%title('error to packetrate for varying delays. Packetsize 1', 'fontsize', 14);
%
%print('-dpdf', '-color', fullfile(pwd, 'lineplot.pdf'));
%print('-deps', '-color', fullfile(pwd, 'lineplot.eps'));

