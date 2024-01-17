#include <stdio.h>

#include "stdio_impl.h"

#ifdef STDIO_NO_FILE_IO
/* "No file system" fgetc, ungetc */

static char ungetbuf[UNGET];
static char ungetcnt = 0;

int fgetc(FILE *fp)
{
    int c;
    extern int getch(void);

    if (fp == stdin) {
        if (ungetcnt) {
            c = ungetbuf[--ungetcnt];
        } else {
            c = getch();
        }
	} else {
		if (fp->ungetcnt) {
			c = fp->ungetbuf[--fp->ungetcnt];
		} else {
			c = fp->source[fp->count];
			if (c == '\0') {
				c = EOF;
			} else {
				++fp->count;
			}
		}
	}
    return c;
}

int ungetc(int c, FILE *fp)
{
	if (c == EOF) {
		return EOF;
	}
    if (fp == stdin) {
		if (ungetcnt != UNGET) {
			ungetbuf[ungetcnt++] = (char)c;
		}
		else {
			return EOF;
		}
    } else {
        if (fp->ungetcnt != UNGET) {
			fp->ungetbuf[fp->ungetcnt++] = (char)c;
		} else {
			return EOF;
		}
    }
    return (unsigned char)c;
}

#endif
