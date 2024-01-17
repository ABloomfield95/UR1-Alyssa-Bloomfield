#include	<ctype.h>

#ifndef isupper

#ifdef _CTYPE_BIT_FUNCS_

__bit
isupper(char c)
#else
int
isupper(int c)
#endif
{
	return c <= 'Z' && c >='A';
}
#endif
