#if	defined(__SIZEOF_DOUBLE__) && defined(__SIZEOF_INT24__) && defined(__SIZEOF_LONG__)

#if __SIZEOF_DOUBLE__ == __SIZEOF_INT24__
#define C1	0x7FFFFF
#define C2	0x7F8000
#define f1_as_uint	(*((__uint24 *)&f1))

#elif __SIZEOF_DOUBLE__ == __SIZEOF_LONG__
#define C1	0x7FFFFFFF
#define C2	0x7F800000
#define f1_as_uint	(*((unsigned long *)&f1))

#else
#error Size of double must be either 24 or 32bits
#endif

#else
#error Missing __SIZEOF_XXX__ macros. __SIZEOF_DOUBLE__, __SIZEOF_INT24__ and __SIZEOF_LONG__ are needed
#endif

int
__isnan (double f1) {
	return (f1_as_uint & C1) > C2;
}

