#include <stdio.h>
#include "stdio_impl.h"

int putchar(int c)
{
	return fputc(c, stdout);
}
