#include <iostream>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
using namespace std;

const int w = 20, h = 20;

class ant{
public:
    int getX() return x;

private:
    int x = 0;
    int y = 0;
};

void printBMP(char name[20], int red[w][h], int green[w][h], int blue[w][h]){
    FILE *f;
    unsigned char *img = NULL;
    int filesize = 54 + 3*w*h;  //w is your image width, h is image height, both int
    if( img )
            free( img );
    img = (unsigned char *)malloc(3*w*h);
    memset(img,0,sizeof(img));
    int x;
    int y;
    int r;
    int g;
    int b;

    for(int i=0; i<w; i++)
    {
            for(int j=0; j<h; j++)
            {
                    x=i; y=(h-1)-j;
                    r = red[i][j]*255;
                    g = green[i][j]*255;
                    b = blue[i][j]*255;
                    if (r > 255) r=255;
                    if (g > 255) g=255;
                    if (b > 255) b=255;
                    img[(x+y*w)*3+2] = (unsigned char)(r);
                    img[(x+y*w)*3+1] = (unsigned char)(g);
                    img[(x+y*w)*3+0] = (unsigned char)(b);
            }
    }

    unsigned char bmpfileheader[14] = {'B','M', 0,0,0,0, 0,0, 0,0, 54,0,0,0};
    unsigned char bmpinfoheader[40] = {40,0,0,0, 0,0,0,0, 0,0,0,0, 1,0, 24,0};
    unsigned char bmppad[3] = {0,0,0};

    bmpfileheader[ 2] = (unsigned char)(filesize    );
    bmpfileheader[ 3] = (unsigned char)(filesize>> 8);
    bmpfileheader[ 4] = (unsigned char)(filesize>>16);
    bmpfileheader[ 5] = (unsigned char)(filesize>>24);

    bmpinfoheader[ 4] = (unsigned char)(       w    );
    bmpinfoheader[ 5] = (unsigned char)(       w>> 8);
    bmpinfoheader[ 6] = (unsigned char)(       w>>16);
    bmpinfoheader[ 7] = (unsigned char)(       w>>24);
    bmpinfoheader[ 8] = (unsigned char)(       h    );
    bmpinfoheader[ 9] = (unsigned char)(       h>> 8);
    bmpinfoheader[10] = (unsigned char)(       h>>16);
    bmpinfoheader[11] = (unsigned char)(       h>>24);

    f = fopen(name,"wb");
    fwrite(bmpfileheader,1,14,f);
    fwrite(bmpinfoheader,1,40,f);
    for(int i=0; i<h; i++)
    {
            fwrite(img+(w*(h-i-1)*3),3,w,f);
            fwrite(bmppad,1,(4-(w*3)%4)%4,f);
    }
    fclose(f); 
}

void loop(int red[w][h], int green[w][h], int blue[w][h]){

    for(int i=1; i<w; i++)
        for(int j=1; j<h; j++)
            green[i][j]+=1*(green[i-1][j-1]);
}

int main() 
{
    cout << "Hello, World!";

    //int w = 100; /* Put here what ever width you want */
    //int h = 100; /* Put here what ever height you want */
    int red[w][h]; 
    int green[w][h];
    int blue[w][h];

    for(int i=0; i<w; i++)
        for(int j=0; j<h; j++)
        {
            red[i][j] = 255; 
            green[i][j] = 255; 
            blue[i][j] = 255; 
        }

    green[0][0] = 0;

    char name[20];

    for(int frame = 0; frame<100; frame++){
        sprintf(name, "dump/%03d.bmp", frame);

        loop(red, green, blue);


        printBMP(name, red, green, blue);
    }


    return 0;
}
