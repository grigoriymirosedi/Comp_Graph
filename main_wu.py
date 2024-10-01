# main_wu.py

from tkinter import Tk, Canvas, PhotoImage
from draw_line import line_wu  # Импортируем только алгоритм Ву

class CanvasControl:
    def __init__(self):
        self.root_main = Tk()
        self.root_main.title("Draw Line - Wu Algorithm")

        win_width = self.root_main.winfo_screenwidth() // 2
        win_height = self.root_main.winfo_screenheight() // 2

        self.canv = Canvas(self.root_main, width=win_width, height=win_height, bg="white")
        self.img = PhotoImage(width=win_width, height=win_height)
        self.canv.create_image((win_width / 2, win_height / 2), image=self.img, state="normal")

        self.root_main.bind("<ButtonRelease-1>", self.draw_wu)

        self.p1_wu = None

        self.canv.pack()
        self.root_main.mainloop()

    def draw_wu(self, event):
        c1 = (0, 0, 255)
        if self.p1_wu is None:
            self.p1_wu = (event.x, event.y)
        else:
            line_wu(self.img, self.p1_wu[0], self.p1_wu[1], event.x, event.y, (255, 255, 255), c1)
            self.p1_wu = None

if __name__ == "__main__":
    cc = CanvasControl()