/* origin: FreeBSD /usr/src/lib/msun/src/math_private.h */
/*
 * ====================================================
 * Copyright (C) 1993 by Sun Microsystems, Inc. All rights reserved.
 *
 * Developed at SunPro, a Sun Microsystems, Inc. business.
 * Permission to use, copy, modify, and distribute this
 * software is freely granted, provided that this notice
 * is preserved.
 * ====================================================
 */

#ifndef _LIBM_H
#define _LIBM_H

#include <stdint.h>
#include <float.h>
#include <math.h>
#ifndef NOCOMPLEX
#include <complex.h>
#endif
#include <endian.h>

#define FORCE_EVAL(x)

/* Get a 32 bit int from a float.  */
typedef union {
	float		f;
	int32_t		s;
	uint32_t	u;
} __xc8_float_word_t;
#define GET_FLOAT_SWORD(w,d)                       \
do {                                              \
	__xc8_float_word.f = (d);                     \
	(w) = __xc8_float_word.s;                     \
} while (0) 
#define GET_FLOAT_UWORD(w,d)                       \
do {                                              \
	__xc8_float_word.f = (d);                     \
	(w) = __xc8_float_word.u;                     \
} while (0) 

/* Set a float from a 32 bit int.  */
#define SET_FLOAT_SWORD(d,w)                       \
do {                                              \
	__xc8_float_word.s = (w);                     \
	(d) = __xc8_float_word.f;                     \
} while (0) 
#define SET_FLOAT_UWORD(d,w)                       \
do {                                              \
	__xc8_float_word.u = (w);                     \
	(d) = __xc8_float_word.f;                     \
} while (0) 

#undef __CMPLX
#undef CMPLX
#undef CMPLXF
#undef CMPLXL

#ifndef NOCOMPLEX
#define __CMPLX(x, y, t) \
	((union { _Complex t __z; t __xy[2]; }){.__xy = {(x),(y)}}.__z)

#define CMPLX(x, y) __CMPLX(x, y, double)
#define CMPLXF(x, y) __CMPLX(x, y, float)
#define CMPLXL(x, y) __CMPLX(x, y, long double)
#endif

/* fdlibm kernel functions */

int    __rem_pio2_large(double*,double*,int,int,int);

int    __rem_pio2(double,double*);
double __sin(double,double,int);
double __cos(double,double);
double __tan(double,double,int);
double __expo2(double);

#ifndef NOCOMPLEX
double complex __ldexp_cexp(double complex,int);
#endif

int __rem_pio2f(float,double*);
float  __sindf(double);
float  __cosdf(double);
float  __tandf(double,int);
float  __expo2f(float);

#ifndef NOCOMPLEX
float complex __ldexp_cexpf(float complex,int);
#endif

int __rem_pio2l(long double, long double *);
long double __sinl(long double, long double, int);
long double __cosl(long double, long double);
long double __tanl(long double, long double, int);

/* polynomial evaluation */
long double __polevll(long double, const long double *, int);
long double __p1evll(long double, const long double *, int);

#endif
