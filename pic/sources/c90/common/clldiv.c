#include	<stdlib.h>

#if __SIZEOF_LONG_LONG__ == 8

#pragma warning disable 1516

lldiv_t
lldiv(long long number, long long denom)
{
	lldiv_t	rv;

	rv.quot = number/denom;
	rv.rem = number%denom;
	return rv;
}

#endif
