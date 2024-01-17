#include	<ctype.h>

#ifndef isspace

#ifdef _CTYPE_BIT_FUNCS_

__bit
isspace(char c)
#else
int
isspace(int c)
#endif
{
	return c == ' ' || c <= 015 && c >= 011;
}

#endif
