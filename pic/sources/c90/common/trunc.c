#include	<math.h>
#include	<limits.h>
#include	<float.h>

#if	__SIZEOF_DOUBLE__ <= __SIZEOF_LONG__
#define	_frndint(x)	((double)(long)(x))
#else
extern double	_frndint(double);
#endif


double
trunc(double x)
{
	double	i;
	int	expon;

	frexp(x, &expon);
	if(expon < 0)
		return 0.0;
	if((unsigned)expon > sizeof(double) * CHAR_BIT - 4)
		return x;		/* already an integer */
	i = _frndint(x);
	return i;
}
