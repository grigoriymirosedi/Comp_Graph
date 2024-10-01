# draw_line.py

def line_bresenhem(img, p1, p2, color):
    """Рисует линию по алгоритму Брезенхема."""
    x1, y1 = p1
    x2, y2 = p2
    dx = abs(x2 - x1)
    dy = abs(y2 - y1)
    sx = 1 if x1 < x2 else -1
    sy = 1 if y1 < y2 else -1
    err = dx - dy

    while True:
        img.put(color, (x1, y1))
        if x1 == x2 and y1 == y2:
            break
        err2 = err * 2
        if err2 > -dy:
            err -= dy
            x1 += sx
        if err2 < dx:
            err += dx
            y1 += sy

def line_wu(img, x1, y1, x2, y2, bg_color, line_color):
    """Рисует линию по алгоритму Ву."""
    def put_pixel(x, y, color, intensity):
        r, g, b = color
        img.put(f'#{int(r * intensity):02x}{int(g * intensity):02x}{int(b * intensity):02x}', (x, y))

    dx = x2 - x1
    dy = y2 - y1
    steps = max(abs(dx), abs(dy))
    x_inc = dx / float(steps)
    y_inc = dy / float(steps)

    x = x1
    y = y1

    for i in range(steps):
        put_pixel(round(x), round(y), line_color, 1 - (i / steps))
        x += x_inc
        y += y_inc
        put_pixel(round(x), round(y), line_color, i / steps)