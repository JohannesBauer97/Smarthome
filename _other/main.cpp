#include <iostream>

/* run this program using the console pauser or add your own getch, system("pause") or input loop */

int main(int argc, char** argv) {
	int RVal = 255;
	int GVal = 0;
	int BVal = 0;
	char valInfo[16];
	char r[3], g[3], b[3];

	if(RVal <= 15){
	    sprintf(r, "0%x", RVal);
	}else{
		sprintf(r, "%x", RVal);
	}

	if(GVal <= 15){
	    sprintf(g, "0%x", GVal);
	}else{
		sprintf(g, "%x", GVal);
	}
	
	if(BVal <= 15){
	    sprintf(b, "0%x", BVal);
	}else{
		sprintf(b, "%x", BVal);
	}
	
	sprintf(valInfo, "#%s%s%s", r, g, b);    

    std::cout << valInfo;
	return 0;
}
