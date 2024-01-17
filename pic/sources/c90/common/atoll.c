#include	<ctype.h>
#include	<stdlib.h>

#pragma warning disable 1516

long long
atoll(register const char * s)
{
	long long		a;
	unsigned char	sign, c;

	do
		c = *s++;
	while(c == ' ' || c == '\t');
	a = 0;
	sign = 0;
	if(c == '-') {
		sign++;
		c = *s++;
	} else if(c == '+')
		c = *s++;
	while(isdigit(c)) {
		a = a*10LL + (unsigned char)(c - '0');
		c = *s++;
	}
	if(sign)
		return -a;
	return a;
}
