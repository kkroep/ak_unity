exp_number = 60;

P= [0, 0];

for i=1:2*exp_number
    P(mod(i,2)+1) =  P(mod(i,2)+1) + poisspdf(i, exp_number);
end

fprintf('exp_number of phontons = %d\n', exp_number);
P-0.5

poisspdf(2000, 1000)
