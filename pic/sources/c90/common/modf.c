#include	<math.h>
#include	<limits.h>
#include	<float.h>

extern double	_frndint(double);

double
modf(double value, double * iptr)
{
#if	__SIZEOF_DOUBLE__ == __SIZEOF_LONG__ && DBL_MAX_EXP == 128

	unsigned	expon;

	if(value >= 0.0 && value < 1.0 || value > -1.0 && value <= 0.0) {
		*iptr = 0;
		return value;
	}
	expon = ((*(unsigned long *)&value >> 23) & 255) - 126;
	if(expon > sizeof(double) * CHAR_BIT - 4) {
		*iptr = value;
		return 0.0;		/* already an integer */
	}
	*iptr = (double)(long)value;
	return value - *iptr;
#elif	__SIZEOF_DOUBLE__ == __SIZEOF_SHORT_LONG__
	*iptr = (double)(__int24)value;
	return value - *iptr;
#else
	*iptr = _frndint(value);
	return value - *iptr;
#endif
}
