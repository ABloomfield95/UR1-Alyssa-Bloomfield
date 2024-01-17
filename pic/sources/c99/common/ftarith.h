/*	Floating point routines.

	Copyright (c) 2012 Microchip Technology Inc. All rights reserved.
*/
// since we use tests on the size of operands, suppress the warnings that
// would be generated
#pragma warning disable 757
#ifdef _OLDLIB
extern unsigned char __ftunpack(__uint24 * arg, unsigned char * exp);
extern void  __ftpack(__uint24 * arg, unsigned char exp);
#else
extern float  __ftpack(__uint24 arg, unsigned char exp, unsigned char sign);
#endif
extern float __ftadd(float, float);
