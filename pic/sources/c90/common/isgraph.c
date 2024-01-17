#include	<ctype.h>

#ifndef isgraph

#ifdef _CTYPE_BIT_FUNCS_

__bit
isgraph(char c)
#else
int
isgraph(int c)
#endif
{
	return c <= 126 && c > ' ';
}

#endif
