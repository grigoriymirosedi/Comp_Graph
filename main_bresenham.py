# main_bresenham.py

from tkinter import Tk, Canvas, PhotoImage
from draw_line import line_bresenhem  # Импортируем только алгоритм Брезенхема

class CanvasControl:
    def __init__(self):
        self.root_main = Tk()
        self.root_main.title("Draw Line - Bresenham Algorithm")

        win_width = self.root_main.winfo_screenwidth() // 2
        win_height = self.root_main.winfo_screenheight() // 2

        self.canv = Canvas(self.root_main, width=win_width, height=win_height, bg="white")
        self.img = PhotoImage(width=win_width, height=win_height)
        self.canv.create_image((win_width / 2, win_height / 2), image=self.img, state="normal")

        self.root_main.bind("<ButtonRelease-1>", self.draw_bresenhem)

        self.p1_be = None

        self.canv.pack()
        self.root_main.mainloop()

    def draw_bresenhem(self, event):
        c1 = "#000000"
        if self.p1_be is None:
            self.p1_be = (event.x, event.y)
        else:
            line_bresenhem(self.img, (self.p1_be[0], self.p1_be[1]), (event.x, event.y), c1)
            self.p1_be = None

if __name__ == "__main__":
    cc = CanvasControl()