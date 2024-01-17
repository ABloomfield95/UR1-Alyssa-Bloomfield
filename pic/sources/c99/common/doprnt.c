/* vfprintf with configurable support for format conversions */
/* This code is specifically for XC8 */
#include <ctype.h>
#include <math.h>
#include <stdarg.h>
#include <stddef.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <inline.h>

/* Configuration options */
#ifdef _VFPF_ALL
#define _VFPF_CONVERT
#define _VFPF_FLAGS
#define _VFPF_WIDTH
#define _VFPF_PRECISION

#define _VFPF_HH
#define _VFPF_H
#define _VFPF_L
#define _VFPF_LL
#define _VFPF_J
#define _VFPF_T
#define _VFPF_Z

#define _VFPF_A
#define _VFPF_C
#define _VFPF_D
#define _VFPF_E
#define _VFPF_F
#define _VFPF_G
#define _VFPF_O
#define _VFPF_N
#define _VFPF_P
#define _VFPF_S
#define _VFPF_U
#define _VFPF_X
#endif

#ifndef ARRAYSIZE
#define ARRAYSIZE(a)	(sizeof(a)/sizeof(a[0]))
#endif
#ifndef CSTRLEN
#define CSTRLEN(s)	(ARRAYSIZE(s)-1)
#endif

/* Flags, precision, width */
#define MINUS_FLAG (1 << 0)
#define ZERO_FLAG (1 << 1)
#define PLUS_FLAG (1 << 2)
#define SPACE_FLAG (1 << 3)
#define POUND_FLAG (1 << 4)
static int flags, prec, width;

#ifdef _VFPF_CONVERT
/* This buffer must be at least 32 bytes long for this code to be safe */
/* Output that would exceed buffer capacity is truncated */
#if defined(_VFPF_A) || defined(_VFPF_E) || defined(_VFPF_F) || defined(_VFPF_G)
#define DBLEN 80
#define EXPLEN 5
#else
#define DBLEN 32
#endif
static char dbuf[DBLEN];

/* Character count */
static int nout;

/* Output the string in dbuf, padded on the left or right */
static _INLINE int pad(FILE *fp, char *buf, int p)
{
    int i, w;

    /* Left justify ? Put out string */
    if (flags & MINUS_FLAG) {
        fputs((const char *)buf, fp);
    }

    /* Put out padding */
    w = (p < 0) ? 0 : p;
    i = 0;
    while (i < w) {
        fputc(' ', fp);
        ++i;
    }

    /* Right justify ? Put out string */
    if (!(flags & MINUS_FLAG)) {
        fputs((const char *)buf, fp);
    }

    return (int)(strlen(buf) + (size_t)w);
}
#endif

#ifdef _VFPF_A
static _INLINE int atoa(FILE *fp, long double f, char c)
{
    char mode, nmode;
    int d, e, i, m, n, ne, p, pp, sign, t, w;
    long double g, h, l, ou, u;

    /* Record sign, get absolute value */
    sign = 0;
    g = f;
    if (g < 0.0) {
        sign = 1;
        g = -g;
    }

    /* Print sign, prefix */
    n = 0;
    w = width;
    if (sign || (flags & PLUS_FLAG)) {
        dbuf[n] = sign ? '-' : '+';
        ++n;
        --w;
    }
    dbuf[n++] = '0';
    dbuf[n++] = isupper((int)c) ? 'X' : 'x';

    /* Catch infinities, NaNs here */
    if (isinf(g)) {
        if (isupper((int)c)) {
            strcpy(&dbuf[n], "INF");
        } else {
            strcpy(&dbuf[n], "inf");
        }
        w -= CSTRLEN("inf");
        return pad(fp, &dbuf[0], w);
    }
    if (isnan(g)) {
        if (isupper((int)c)) {
            strcpy(&dbuf[n], "NAN");
        } else {
            strcpy(&dbuf[n], "nan");
        }
        w -= CSTRLEN("inf");
        return pad(fp, &dbuf[0], w);
    }

    /* First find the largest power of 2 not larger than number to print */
    u = 1.0;
    e = 0;
    if (!(g == 0.0)) {
        while (!(g < (u*2.0))) {
            u = u*2.0;
            ++e;
        }
        while (g < u) {
            u = u/2.0;
            --e;
        }
    }

    /* Get precision */
    p = (prec < 0) ? 6 : prec;

    /* Hex places, total */
    m = p + 1;

    /* Go through the conversion once to get to the rounding step */
    i = 0;
    h = g;
    ou = u;
    while (i < m) {
        l = floor(h/u);
        d = (int)l;
        h -= l*u;
        u = u/16.0;
        ++i;
    }
    
    /* Remainder >= halfway ? */
    l = u*8.0;
    if (h < l) {
        l = 0.0;
    } else {
        /* On tie choose even number */
        if ((h == l) && !(d % 2)) {
            l = 0.0;
        }
    }

    /* Round */
    h = g + l;
    
    /* Convert again, after rounding */
    u = ou;
    ne = 0;
    pp = 0;
    t = 0;
    i = 0;
    while ((i < m) && (n < (DBLEN - EXPLEN))) {
        l = floor(h/u);
        d = (int)l;
        if (!(flags & POUND_FLAG) && !d && (ne < 0)) {
            ++t;
        } else {
            if (!pp && (ne < 0)) {
                dbuf[n++] = '.';
                --w;
                pp = 1;
            }
            while (t) {
                dbuf[n++] = '0';
                --w;
                --t;
            }
            d = (d < 10) ? (int)'0' + d : (int)'a' + (d - 10);
            if (isupper((int)c) && isalpha(d)) {
                d = toupper(d);
            }
            dbuf[n++] = (char)d;
            --w;
        }
        h -= l*u;
        u = u/16.0;
        --ne;
        ++i;
    }
    if (!pp && (flags & POUND_FLAG)) {
        dbuf[n++] = '.';
    }
    dbuf[n] = '\0';

    /* Convert exponent */
    i = sizeof(dbuf) - 1;
    dbuf[i] = '\0';
    sign = 0;
    if (e < 0) {
        sign = 1;
        e = -e;
    }
    p = 1;
    while (e || (0 < p)) {
        --i;
        dbuf[i] = '0' + (e % 10);
        e = e / 10;
        --p;
        --w;
    }
    --i;
    dbuf[i] = sign ? '-' : '+';
    --w;
    --i;
    dbuf[i] = isupper((int)c) ? 'P' : 'p';
    --w;
    strcpy(&dbuf[n], &dbuf[i]);

    /* Put out padded string */
    return pad(fp, &dbuf[0], w);
}
#endif

#ifdef _VFPF_C
static _INLINE int ctoa(FILE *fp, char c)
{
    int l, w;

    /* Get width */
    w = width ? width - 1 : width;

    /* Left justify ? Put out character */
    if (flags & MINUS_FLAG) {
        fputc(c, fp);
    }
    /* Put out padding */
    w = (w < 0) ? 0 : w;
    l = 0;
    while (l < w) {
        fputc(' ', fp);
        ++l;
    }
    /* Right justify ? Put out string */
    if (!(flags & MINUS_FLAG)) {
        fputc(c, fp);
    }

    return l+1;
}
#endif

#ifdef _VFPF_D
static _INLINE int dtoa(FILE *fp, long long d)
{
    int i, p, s, w;
    long long n;

    /* Record sign, get absolute value */
    n = d;
    s = n < 0 ? 1 : 0;
    if (s) {
        n = -n;
    }

    /* Adjust flags, precision, width */
    if (!(prec < 0)) {
        flags &= ~ZERO_FLAG;
    }
    p = (0 < prec) ? prec : 1;
    w = width;
    if (s || (flags & PLUS_FLAG)) {
        --w;
    }

    /* Convert to decimal, possibly filling on the left with zeroes */
    i = sizeof(dbuf) - 1;
    dbuf[i] = '\0';
    while (!(i < 1) && (n || (0 < p) || ((0 < w) && (flags & ZERO_FLAG)))) {
        --i;
        dbuf[i] = (char)((int)'0' + abs(n % 10));
        --p;
        --w;
        n = n / 10;
    }

    /* Display sign if required */
    if (s || (flags & PLUS_FLAG)) {
        --i;
        dbuf[i] = s ? '-' : '+';
    }

    /* Put out padded string */
    return pad(fp, &dbuf[i], w);
}
#endif

#if defined(_VFPF_E) || defined(_VFPF_F) || defined(_VFPF_G)
static _INLINE int efgtoa(FILE *fp, long double f, char c)
{
    char mode, nmode;
    int d, e, i, m, n, ne, p, pp, sign, t, w;
    long double g, h, l, ou, u;

    /* Record sign, get absolute value */
    sign = 0;
    g = f;
    if (g < 0.0) {
        sign = 1;
        g = -g;
    }

    /* Print sign */
    n = 0;
    w = width;
    if (sign || (flags & PLUS_FLAG)) {
        dbuf[n] = sign ? '-' : '+';
        ++n;
        --w;
    }

    /* Catch infinities, NaNs here */
    if (isinf(g)) {
        if (isupper((int)c)) {
            strcpy(&dbuf[n], "INF");
        } else {
            strcpy(&dbuf[n], "inf");
        }
        w -= CSTRLEN("inf");
        return pad(fp, &dbuf[0], w);
    }
    if (isnan(g)) {
        if (isupper((int)c)) {
            strcpy(&dbuf[n], "NAN");
        } else {
            strcpy(&dbuf[n], "nan");
        }
        w -= CSTRLEN("inf");
        return pad(fp, &dbuf[0], w);
    }

    /* First find the largest power of 10 not larger than number to print */
    u = 1.0;
    e = 0;
    if (!(g == 0.0)) {
        while (!(g < (u*10.0))) {
            u = u*10.0;
            ++e;
        }
        while (g < u) {
            u = u/10.0;
            --e;
        }
    }

    /* Get mode, precision */
    mode = (char)tolower((int)c);
    nmode = mode;
    if (mode == 'g') {
		if (prec == 0) {
			prec = 1;
		}
        p = (0 < prec) ? prec : 6;
    } else {
        p = (prec < 0) ? 6 : prec;
    }

    /* Choose e or f mode from g mode */
    if (mode == 'g') {
        if (!(e < -4) && !((p - 1) < e)) {
            nmode = 'f';
        } else {
            nmode = 'e';
        }
    }

    /* Decimal places or significant digits */
    m = p;
    if (!(mode == 'g') || ((nmode == 'f') && (e < 0))) {
        ++m;
    }

    /* Adjust starting exponent, string length for 'f' conversions */
    if (nmode == 'f') {
        if (e < 0) {
            u = 1.0;
            e = 0;
        }
        if (!(mode == 'g')) {
            m += e;
        }
    }

    /* Go through the conversion once to get to the rounding step */
    i = 0;
    h = g;
    ou = u;
    while (i < m) {
        l = floor(h/u);
        d = (int)l;
        h -= l*u;
        u = u/10.0;
        ++i;
    }
    
    /* Remainder >= halfway ? */
    l = u*5.0;
    if (h < l) {
        l = 0.0;
    } else {
        /* On tie choose even number */
        if ((h == l) && !(d % 2)) {
            l = 0.0;
        }
    }

    /* Round */
    h = g + l;
    /* Has rounding increased the power above 10^0? */
	if (h >= (ou*10.0)) {
		e++;
		ou *= 10.0;
		if (nmode == 'f') {
			// the increase in power will only affect the number of digits in 'f' mode
			m++;
		}
	}
    
    /* Convert again, after rounding */
    u = ou;
    ne = (nmode == 'e') ? 0 : e;
    pp = 0;
    t = 0;
    i = 0;
    while ((i < m) && (n < (DBLEN - EXPLEN))) {
        l = floor(h/u);
        d = (int)l;
        if (!(flags & POUND_FLAG) && !d && (mode == 'g') && (ne < 0)) {
            ++t;
        } else {
            if (!pp && (ne < 0)) {
                dbuf[n++] = '.';
                --w;
                pp = 1;
            }
            while (t) {
                dbuf[n++] = '0';
                --w;
                --t;
            }
            dbuf[n++] = (char)((int)'0' + d);
            --w;
        }
        h -= l*u;
        u = u/10.0;
        --ne;
        ++i;
    }
    if (!pp && (flags & POUND_FLAG)) {
        dbuf[n++] = '.';
    }
    dbuf[n] = '\0';

    /* Convert exponent */
    if (nmode == 'e') {
        i = sizeof(dbuf) - 1;
        dbuf[i] = '\0';
        sign = 0;
        if (e < 0) {
            sign = 1;
            e = -e;
        }
        p = 2;
        while (e || (0 < p)) {
            --i;
            dbuf[i] = '0' + (e % 10);
            e = e / 10;
            --p;
            --w;
        }
        --i;
        dbuf[i] = sign ? '-' : '+';
        --w;
        --i;
        dbuf[i] = isupper((int)c) ? 'E' : 'e';
        --w;
        strcpy(&dbuf[n], &dbuf[i]);
    }

    /* Put out padded string */
    return pad(fp, &dbuf[0], w);
}
#endif

#ifdef _VFPF_O
static _INLINE int otoa(FILE *fp, unsigned long long d)
{
    int i, p, t, w;
    unsigned long long n;

    /* Adjust flags, precision, width */
    if (!(prec < 0)) {
        flags &= ~ZERO_FLAG;
    }
    p = (0 < prec) ? prec : 1;
    w = width;

    /* Convert to octal, possibly filling on the left with zeroes */
    n = d;
    i = sizeof(dbuf) - 1;
    dbuf[i] = '\0';
    t = 0;
    while (!(i < 1) && (n || (0 < p) || ((0 < w) && (flags & ZERO_FLAG)))) {
        --i;
        t = n & 07;
        dbuf[i] = (char)((int)'0' + t);
        --p;
        --w;
        n = n >> 3;
    }

    /* Display prefix if required */
    if ((flags & POUND_FLAG) && t) {
        --i;
        dbuf[i] = '0';
        --w;
    }

    /* Put out padded string */
    return pad(fp, &dbuf[i], w);
}
#endif

#ifdef _VFPF_S
static _INLINE int stoa(FILE *fp, char *s)
{
    char *cp, nuls[] = "(null)";
    int i, l, p, w;

    /* Check for null string */
    cp = s;
    if (!cp) {
        cp = nuls;
    }

    /* Get length, precision, width */
    l = (int)strlen(cp);
    p = prec;
    l = (!(p < 0) && (p < l)) ? p : l;
    p = l;
    w = width;

    /* Right justify, pad on left ? */
    if (!(flags & MINUS_FLAG)) {
        while (l < w) {
            fputc(' ', fp);
            ++l;
        }
    }

    /* Put out string */
    i = 0;
    while (i < p) {
        fputc(*cp, fp);
        ++cp;
        ++i;
    }

    /* Left justify, pad on right ? */
    if (flags & MINUS_FLAG) {
        while (l < w) {
            fputc(' ', fp);
            ++l;
        }
    }

    return l;
}
#endif

#ifdef _VFPF_U
static _INLINE int utoa(FILE *fp, unsigned long long d)
{
    int i, p, w;
    unsigned long long n;

    /* Adjust flags, precision, width */
    if (!(prec < 0)) {
        flags &= ~ZERO_FLAG;
    }
    p = (0 < prec) ? prec : 1;
    w = width;

    /* Convert to decimal, possibly filling on the left with zeroes */
    n = d;
    i = sizeof(dbuf) - 1;
    dbuf[i] = '\0';
    while (i && (n || (0 < p) || ((0 < w) && (flags & ZERO_FLAG)))) {
        --i;
        dbuf[i] = '0' + (n % 10);
        --p;
        --w;
        n = n / 10;
    }

    /* Put out padded string */
    return pad(fp, &dbuf[i], w);
}
#endif

#if defined(_VFPF_X) || defined(_VFPF_P)
static _INLINE int xtoa(FILE *fp, unsigned long long d, char x)
{
    int c, i, p, w;
    unsigned long long n;

    /* Adjust, flags, precision, width */
    if (!(prec < 0)) {
        flags &= ~ZERO_FLAG;
    }
    p = (0 < prec) ? prec : 1;
    w = width;
    if (flags & POUND_FLAG) {
        w -= 2;
    }

    /* Convert to hexadecimal, possibly filling on the left with zeroes */
    n = d;
    i = sizeof(dbuf) - 1;
    dbuf[i] = '\0';
    while (!(i < 2) && (n || (0 < p) || ((0 < w) && (flags & ZERO_FLAG)))) {
        --i;
        c = n & 0x0f;
        c = (c < 10) ? (int)'0' + c : (int)'a' + (c - 10);
        if (isupper((int)x) && isalpha(c)) {
            c = toupper(c);
        }
        dbuf[i] = (char)c;
        --p;
        --w;
        n = n >> 4;
    }

    /* Display prefix if required */
    if (flags & POUND_FLAG) {
        --i;
        dbuf[i] = x;
        --i;
        dbuf[i] = '0';
    }

    /* Put out padded string */
    return pad(fp, &dbuf[i], w);
}
#endif

/* Consume and convert the next part of the format string */
#ifdef _VFPF_CONVERT
static _INLINE int vfpfcnvrt(FILE *fp, char *fmt[], va_list ap)
{
    char c, *cp, ct[3];
    int done, i;
    long long ll;
    unsigned long long llu;
    long double f;
    void *vp;

    /* Conversion ? */
    if ((*fmt)[0] == '%') {
        ++*fmt;

        flags = width = 0;
        prec = -1;

#ifdef _VFPF_FLAGS
        /* Get flags */
        done = 0;
        while (!done) {
            switch ((*fmt)[0]) {
                case '-' :
                    flags |= MINUS_FLAG;
                    ++*fmt;
                    break;
                case '0' :
                    flags |= ZERO_FLAG;
                    ++*fmt;
                    break;
                case '+' :
                    flags |= PLUS_FLAG;
                    ++*fmt;
                    break;
                case ' ' :
                    flags |= SPACE_FLAG;
                    ++*fmt;
                    break;
                case '#' :
                    flags |= POUND_FLAG;
                    ++*fmt;
                    break;
                default:
                    done = 1;
                    break;
            }
        }
        if (flags & MINUS_FLAG) {
            flags &= ~ZERO_FLAG;
        }
#endif

#ifdef _VFPF_WIDTH
        /* Get field width */
        if ((*fmt)[0] == '*') {
            ++*fmt;
            width = va_arg(ap, int);
            if (width < 0) {
                flags |= MINUS_FLAG;
                width = -width;
            }
        } else {
            width = atoi(*fmt);
            while (isdigit((*fmt)[0])) {
                ++*fmt;
            }
        }
#endif

#ifdef _VFPF_PRECISION
        /* Get precision */
        if ((*fmt)[0] == '.') {
            prec = 0;
            ++*fmt;
            if ((*fmt)[0] == '*') {
                ++*fmt;
                prec = va_arg(ap, int);
            } else {
                prec = atoi(*fmt);
                while (isdigit((*fmt)[0])) {
                    ++*fmt;
                }
            }
        }
#endif

#if defined(_VFPF_A) || defined(_VFPF_E) || defined(_VFPF_F) || defined(_VFPF_G)
        /* Case-folded conversion types */
        ct[0] = (char)tolower((int)(*fmt)[0]);
        if (ct[0]) {
            ct[1] = (char)tolower((int)(*fmt)[1]);
            if (ct[1]) {
                ct[2] = (char)tolower((int)(*fmt)[2]);
            }
        }
#endif

#ifdef _VFPF_A
        /* 'a' style (hex) floating point */
        if (ct[0] == 'a') {

            c = (*fmt)[0];
            ++*fmt;
            f = (long double)va_arg(ap, double);
                        
            return atoa(fp, f, c);
        }
        if (!strncmp(ct, "la", CSTRLEN("la"))) {

            c = (*fmt)[1];
            if (isupper((int)(*fmt)[0])) {
                f = va_arg(ap, long double);
            } else {
                f = (long double)va_arg(ap, double);
            }
            *fmt += CSTRLEN("la");
                        
            return atoa(fp, f, c);
        }
#endif

#ifdef _VFPF_C
        /* Character */
        if (*fmt[0] == 'c') {
            ++*fmt;
            c = (unsigned char)va_arg(ap, int);
            return ctoa(fp, c);
        }
#endif

#ifdef _VFPF_D
#ifdef _VFPF_HH
        /* Character decimal integer */
        if (!strncmp(*fmt, "hhd", CSTRLEN("hhd")) || \
            !strncmp(*fmt, "hhi", CSTRLEN("hhi"))) {

            *fmt += CSTRLEN("hhd");
            ll = (long long)(signed char)va_arg(ap, int);
                        
            return dtoa(fp, ll);
        }
#endif

#ifdef _VFPF_H
        /* Short decimal integer */
        if (!strncmp(*fmt, "hd", CSTRLEN("hd")) || \
            !strncmp(*fmt, "hi", CSTRLEN("hi"))) {

            *fmt += CSTRLEN("hd");
            ll = (long long)(short)va_arg(ap, int);
                        
            return dtoa(fp, ll);
        }
#endif

        /* Decimal integer */
        if ((*fmt[0] == 'd') || (*fmt[0] == 'i')) {

            ++*fmt;
            ll = (long long)va_arg(ap, int);
                        
            return dtoa(fp, ll);
        }

#ifdef _VFPF_L
        /* Long decimal integer */
        if (!strncmp(*fmt, "ld", CSTRLEN("ld")) || \
            !strncmp(*fmt, "li", CSTRLEN("li"))) {

            *fmt += CSTRLEN("ld");
            ll = (long long)va_arg(ap, long);
                        
            return dtoa(fp, ll);
        }
#endif

#ifdef _VFPF_LL
        /* Long long decimal integer */
        if (!strncmp(*fmt, "lld", CSTRLEN("lld")) || \
            !strncmp(*fmt, "lli", CSTRLEN("lli"))) {

            *fmt += CSTRLEN("lld");
            ll = va_arg(ap, long long);
                        
            return dtoa(fp, ll);
        }
#endif

#ifdef _VFPF_J
        /* intmax_t decimal integer */
        if (!strncmp(*fmt, "jd", CSTRLEN("jd")) || \
            !strncmp(*fmt, "ji", CSTRLEN("ji"))) {

            *fmt += CSTRLEN("jd");
            ll = (long long)va_arg(ap, intmax_t);
                        
            return dtoa(fp, ll);
        }
#endif

#ifdef _VFPF_T
        /* ptrdiff_t decimal integer */
        if (!strncmp(*fmt, "td", CSTRLEN("td")) || \
            !strncmp(*fmt, "ti", CSTRLEN("ti"))) {

            *fmt += CSTRLEN("td");
            ll = (long long)va_arg(ap, ptrdiff_t);
                        
            return dtoa(fp, ll);
        }
#endif

#ifdef _VFPF_Z
        /* size_t decimal integer */
        if (!strncmp(*fmt, "zd", CSTRLEN("zd")) || \
            !strncmp(*fmt, "zi", CSTRLEN("zi"))) {

            *fmt += CSTRLEN("zd");
            ll = (long long)va_arg(ap, size_t);
                        
            return dtoa(fp, ll);
        }
#endif
#endif

#ifdef _VFPF_E
        /* 'e' style floating point */
        if (ct[0] == 'e') {

            c = (*fmt)[0];
            ++*fmt;
            f = (long double)va_arg(ap, double);
                        
            return efgtoa(fp, f, c);
        }
        if (!strncmp(ct, "le", CSTRLEN("le"))) {

            c = (*fmt)[1];
            if (isupper((int)(*fmt)[0])) {
                f = va_arg(ap, long double);
            } else {
                f = (long double)va_arg(ap, double);
            }
            *fmt += CSTRLEN("lf");
                        
            return efgtoa(fp, f, c);
        }
#endif

#ifdef _VFPF_F
        /* 'f' style floating point */
        if (ct[0] == 'f') {

            c = (*fmt)[0];
            ++*fmt;
            f = (long double)va_arg(ap, double);
                        
            return efgtoa(fp, f, c);
        }
        if (!strncmp(ct, "lf", CSTRLEN("lf"))) {

            c = (*fmt)[1];
            if (isupper((int)(*fmt)[0])) {
                f = va_arg(ap, long double);
            } else {
                f = (long double)va_arg(ap, double);
            }
            *fmt += CSTRLEN("lf");
                        
            return efgtoa(fp, f, c);
        }
#endif

#ifdef _VFPF_G
        /* 'g' style floating point */
        if (ct[0] == 'g') {

            c = (*fmt)[0];
            ++*fmt;
            f = (long double)va_arg(ap, double);
                        
            return efgtoa(fp, f, c);
        }
        if (!strncmp(ct, "lg", CSTRLEN("lg"))) {

            c = (*fmt)[1];
            if (isupper((int)(*fmt)[0])) {
                f = va_arg(ap, long double);
            } else {
                f = (long double)va_arg(ap, double);
            }
            *fmt += CSTRLEN("lg");
                        
            return efgtoa(fp, f, c);
        }
#endif

#ifdef _VFPF_O
#ifdef _VFPF_HH
        /* Character octal integer */
        if (!strncmp(*fmt, "hho", CSTRLEN("hho"))) {

            *fmt += CSTRLEN("hho");
            llu = (unsigned long long)(unsigned char)va_arg(ap, int);
                        
            return otoa(fp, llu);
        }
#endif

#ifdef _VFPF_H
        /* Short octal integer */
        if (!strncmp(*fmt, "ho", CSTRLEN("ho"))) {

            *fmt += CSTRLEN("ho");
            llu = (unsigned long long)(unsigned short)va_arg(ap, int);
                        
            return otoa(fp, llu);
        }
#endif

        /* Octal integer */
        if (*fmt[0] == 'o') {

            ++*fmt;
            llu = (unsigned long long)va_arg(ap, unsigned int);
                        
            return otoa(fp, llu);
        }

#ifdef _VFPF_L
        /* Long octal integer */
        if (!strncmp(*fmt, "lo", CSTRLEN("lo"))) {

            *fmt += CSTRLEN("lo");
            llu = (unsigned long long)va_arg(ap, unsigned long);
                        
            return otoa(fp, llu);
        }
#endif

#ifdef _VFPF_LL
        /* Long long octal integer */
        if (!strncmp(*fmt, "llo", CSTRLEN("llo"))) {

            *fmt += CSTRLEN("llo");
            llu = va_arg(ap, unsigned long long);
                        
            return otoa(fp, llu);
        }
#endif

#ifdef _VFPF_J
        /* uintmax_t octal integer */
        if (!strncmp(*fmt, "jo", CSTRLEN("jo"))) {

            *fmt += CSTRLEN("jo");
            llu = (unsigned long long)va_arg(ap, uintmax_t);
                        
            return otoa(fp, llu);
        }
#endif

#ifdef _VFPF_T
        /* ptrdiff_t octal integer */
        if (!strncmp(*fmt, "to", CSTRLEN("to"))) {

            *fmt += CSTRLEN("to");
            llu = (unsigned long long)va_arg(ap, ptrdiff_t);
                        
            return otoa(fp, llu);
        }
#endif

#ifdef _VFPF_Z
        /* size_t octal integer */
        if (!strncmp(*fmt, "zo", CSTRLEN("zo"))) {

            *fmt += CSTRLEN("zo");
            llu = (unsigned long long)va_arg(ap, size_t);
                        
            return otoa(fp, llu);
        }
#endif
#endif

        /* Character count */
#ifdef _VFPF_N

#ifdef _VFPF_HH
        if (!strncmp(*fmt, "hhn", CSTRLEN("hhn"))) {

            *fmt += CSTRLEN("hhn");
            vp = (void *)va_arg(ap, char *);
            *(char *)vp = (char)nout;
            return 0;
        }
#endif

#ifdef _VFPF_H
        if (!strncmp(*fmt, "hn", CSTRLEN("hn"))) {

            *fmt += CSTRLEN("hn");
            vp = (void *)va_arg(ap, short *);
            *(short *)vp = (short)nout;
            return 0;
        }
#endif

        if (*fmt[0] == 'n') {
            ++*fmt;
            vp = (void *)va_arg(ap, int *);
            *(int *)vp = nout;
            return 0;
        }

#ifdef _VFPF_L
        if (!strncmp(*fmt, "ln", CSTRLEN("ln"))) {

            *fmt += CSTRLEN("ln");
            vp = (void *)va_arg(ap, long *);
            *(long *)vp = (long)nout;
            return 0;
        }
#endif

#ifdef _VFPF_LL
        if (!strncmp(*fmt, "lln", CSTRLEN("lln"))) {

            *fmt += CSTRLEN("lln");
            vp = (void *)va_arg(ap, long long *);
            *(long long *)vp = (long long)nout;
            return 0;
        }
#endif

#ifdef _VFPF_J
        if (!strncmp(*fmt, "jn", CSTRLEN("jn"))) {

            *fmt += CSTRLEN("jn");
            vp = (void *)va_arg(ap, uintmax_t *);
            *(uintmax_t *)vp = (uintmax_t)nout;
            return 0;
        }
#endif

#ifdef _VFPF_T
        if (!strncmp(*fmt, "tn", CSTRLEN("tn"))) {

            *fmt += CSTRLEN("tn");
            vp = (void *)va_arg(ap, ptrdiff_t *);
            *(ptrdiff_t *)vp = (ptrdiff_t)nout;
            return 0;
        }
#endif

#ifdef _VFPF_Z
        if (!strncmp(*fmt, "zn", CSTRLEN("zn"))) {

            *fmt += CSTRLEN("zn");
            vp = (void *)va_arg(ap, size_t *);
            *(size_t *)vp = (size_t)nout;
            return 0;
        }
#endif

#endif

#ifdef _VFPF_P
        /* Pointer */
        if (*fmt[0] == 'p') {

            ++*fmt;
            llu = (unsigned long long)(uintptr_t)va_arg(ap, void *);
                        
            return xtoa(fp, llu, 'x');
        }
#endif

#ifdef _VFPF_S
        /* String */
        if (*fmt[0] == 's' || !strncmp(*fmt, "lls", CSTRLEN("lls"))) {

			*fmt += *fmt[0] == 's' ? 1 : CSTRLEN("lls");
            cp = va_arg(ap, char *);

            return stoa(fp, cp);
        }
#endif

#ifdef _VFPF_U
#ifdef _VFPF_HH
        /* Unsigned character decimal integer */
        if (!strncmp(*fmt, "hhu", CSTRLEN("hhu"))) {

            *fmt += CSTRLEN("hhu");
            llu = (unsigned long long)(unsigned char)va_arg(ap, int);
                        
            return utoa(fp, llu);
        }
#endif

#ifdef _VFPF_H
        /* Unsigned short decimal integer */
        if (!strncmp(*fmt, "hu", CSTRLEN("hu"))) {

            *fmt += CSTRLEN("hu");
            llu = (unsigned long long)(unsigned short)va_arg(ap, int);
                        
            return utoa(fp, llu);
        }
#endif

        /* Unsigned decimal integer */
        if (*fmt[0] == 'u') {

            ++*fmt;
            llu = (unsigned long long)va_arg(ap, unsigned int);
                        
            return utoa(fp, llu);
        }

#ifdef _VFPF_L
        /* Unsigned long decimal integer */
        if (!strncmp(*fmt, "lu", CSTRLEN("lu"))) {

            *fmt += CSTRLEN("lu");
            llu = (unsigned long long)va_arg(ap, unsigned long);
                        
            return utoa(fp, llu);
        }
#endif

#ifdef _VFPF_LL
        /* Unsigned long long decimal integer */
        if (!strncmp(*fmt, "llu", CSTRLEN("llu"))) {

            *fmt += CSTRLEN("llu");
            llu = va_arg(ap, unsigned long long);
                        
            return utoa(fp, llu);
        }
#endif

#ifdef _VFPF_J
        /* uintmax_t decimal integer */
        if (!strncmp(*fmt, "ju", CSTRLEN("ju"))) {

            *fmt += CSTRLEN("ju");
            llu = (unsigned long long)va_arg(ap, uintmax_t);
                        
            return utoa(fp, llu);
        }
#endif

#ifdef _VFPF_T
        /* ptrdiff_t decimal integer */
        if (!strncmp(*fmt, "tu", CSTRLEN("tu"))) {

            *fmt += CSTRLEN("tu");
            llu = (unsigned long long)va_arg(ap, ptrdiff_t);
                        
            return utoa(fp, llu);
        }
#endif

#ifdef _VFPF_Z
        /* size_t decimal integer */
        if (!strncmp(*fmt, "zu", CSTRLEN("zu"))) {

            *fmt += CSTRLEN("zu");
            llu = (unsigned long long)va_arg(ap, size_t);
                        
            return utoa(fp, llu);
        }
#endif
#endif

#ifdef _VFPF_X
#ifdef _VFPF_HH
        /* Character hexadecimal integer */
        if (!strncmp(*fmt, "hhx", CSTRLEN("hhx")) || \
            !strncmp(*fmt, "hhX", CSTRLEN("hhX"))) {

            c = (*fmt)[2];
            *fmt += CSTRLEN("hhx");
            llu = (unsigned long long)(unsigned char)va_arg(ap, int);
                        
            return xtoa(fp, llu, c);
        }
#endif

#ifdef _VFPF_H
        /* Short hexadecimal integer */
        if (!strncmp(*fmt, "hx", CSTRLEN("hx")) || \
            !strncmp(*fmt, "hX", CSTRLEN("hX"))) {

            c = (*fmt)[1];
            *fmt += CSTRLEN("hx");
            llu = (unsigned long long)(unsigned short)va_arg(ap, int);
                        
            return xtoa(fp, llu, c);
        }
#endif

        /* Hexadecimal integer */
        if ((*fmt[0] == 'x') || (*fmt[0] == 'X')) {

            c = (*fmt)[0];
            ++*fmt;
            llu = (unsigned long long)va_arg(ap, unsigned int);
                        
            return xtoa(fp, llu, c);
        }

#ifdef _VFPF_L
        /* Long hexadecimal integer */
        if (!strncmp(*fmt, "lx", CSTRLEN("lx")) || \
            !strncmp(*fmt, "lX", CSTRLEN("lX"))) {

            c = (*fmt)[1];
            *fmt += CSTRLEN("lx");
            llu = (unsigned long long)va_arg(ap, unsigned long);
                        
            return xtoa(fp, llu, c);
        }
#endif

#ifdef _VFPF_LL
        /* Long long hexadecimal integer */
        if (!strncmp(*fmt, "llx", CSTRLEN("llx")) || \
            !strncmp(*fmt, "llX", CSTRLEN("llX"))) {

            c = (*fmt)[2];
            *fmt += CSTRLEN("llx");
            llu = va_arg(ap, unsigned long long);
                        
            return xtoa(fp, llu, c);
        }
#endif

#ifdef _VFPF_J
        /* uintmax_t hexadecimal integer */
        if (!strncmp(*fmt, "jx", CSTRLEN("jx")) || \
            !strncmp(*fmt, "jX", CSTRLEN("jX"))) {

            c = (*fmt)[1];
            *fmt += CSTRLEN("jx");
            llu = (unsigned long long)va_arg(ap, uintmax_t);
                        
            return xtoa(fp, llu, c);
        }
#endif

#ifdef _VFPF_T
        /* ptrdiff_t hexadecimal integer */
        if (!strncmp(*fmt, "tx", CSTRLEN("tx")) || \
            !strncmp(*fmt, "tX", CSTRLEN("tX"))) {

            c = (*fmt)[1];
            *fmt += CSTRLEN("tx");
            llu = (unsigned long long)va_arg(ap, ptrdiff_t);
                        
            return xtoa(fp, llu, c);
        }
#endif

#ifdef _VFPF_Z
        /* size_t hexadecimal integer */
        if (!strncmp(*fmt, "zx", CSTRLEN("zx")) || \
            !strncmp(*fmt, "zX", CSTRLEN("zX"))) {

            c = (*fmt)[1];
            *fmt += CSTRLEN("zx");
            llu = (unsigned long long)va_arg(ap, size_t);
                        
            return xtoa(fp, llu, c);
        }
#endif
#endif

        /* 'Escaped' '%' character */
        if ((*fmt)[0] == '%') {
            ++*fmt;
            fputc((int)'%', fp);
            return 1;
        }

        /* Unrecognized conversion */
        ++*fmt;
        return 0;
    }

    /* No conversion, just intervening text */
    fputc((int)(*fmt)[0], fp);
    ++*fmt;
    return 1;
}
#endif

int vfprintf(FILE *fp, const char *fmt, va_list ap)
{
#ifdef _VFPF_CONVERT
    char *cfmt;

    cfmt = (char *)fmt;
    nout = 0;
    while (*cfmt) {
        nout += vfpfcnvrt(fp, &cfmt, ap);
    }
    return nout;
#else
    return fputs(fmt, fp);
#endif
}
