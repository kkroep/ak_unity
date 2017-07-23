p = 2.33e3;
DC_sens = 0.14e-6;
tw = 525e-6;
g = 9.8;
k = 1000;
phi = 0.9546951; %Number of rads

%%
m = k*DC_sens/g
fo = 1/(2*pi)*sqrt(k/m);

%% Data needed for the pyramid
wm = (-sqrt(2)*p*tw^2 + sqrt(-(2/3)*p^2*tw^4 + 4*p*tw*m ) )/(2*p*tw);
wfront = wm + 2*tw/tan(phi);
ratio = wfront/wm;
height = tw;

%% length of side arms
wb = 50e-6;
tb = 5e-6;
E = 1.7e11;

lb = ((4*E*wb*tb^3)/(k))^(1/3);

%% damping stuff
zeta = 0.707;
kb = 1.38e-23;
T = 293;
vis = 1.98e-5;
pas = 101325;
%damping
fz = zeta*2*sqrt(m*k);
bd = fz;
%noise
Mechnoise = 4*kb*T*bd;
%distance bot and top
dt = ((384*wfront^4*vis)/(pi^6*bd))^(1/3);
db = ((384*wm^4*vis)/(pi^6*bd))^(1/3);
%1/caps value top and bot
kt = (64*wfront^2*pas)/(pi^4*dt);
kbot = (64*wm^2*pas)/(pi^4*db);

%single sided damping
dd = ((384*wfront^4*vis)/(pi^6*bd) + (384*wm^4*vis)/(pi^6*bd))^(1/3);
kk = (64*wm^2*pas)/(pi^4*dd);
caps = 1/kk;

%%error prop
dw = 5e-6;
dm =  abs(2*p*wm*tw + sqrt(2)*p*tw^2)*dw;
dfo = 1/(4*pi) * sqrt(k)/m^1.5 * dm;
dDC = g/k*dm;
dfz = zeta*sqrt(k/m)*dm;
dnoise = 4*kb*T*dfz;


%percentage change
pw = dw/wm*100;
pm = dm/m*100;
pfo = dfo/fo*100;
pDC = dDC/DC_sens * 100;
pfz = dfz/fz*100;
pnoise = dnoise/Mechnoise*100;