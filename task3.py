import cv2
import numpy as np
import matplotlib.pyplot as plt
from matplotlib.widgets import Slider

# Функция для обновления изображения с новыми параметрами HSV
def update(val):
    h = h_slider.val
    s = s_slider.val
    v = v_slider.val

    # Создание копии изображения HSV
    hsv_mod = hsv_image.copy()

    # Изменение оттенка, насыщенности и яркости
    hsv_mod[:, :, 0] = (hsv_mod[:, :, 0].astype(np.float32) * h).astype(np.uint8) % 180  # Оттенок (0-180)
    hsv_mod[:, :, 1] = np.clip(hsv_mod[:, :, 1].astype(np.float32) * s, 0, 255).astype(np.uint8)  # Насыщенность (0-255)
    hsv_mod[:, :, 2] = np.clip(hsv_mod[:, :, 2].astype(np.float32) * v, 0, 255).astype(np.uint8)  # Яркость (0-255)

    # Преобразование обратно в RGB
    rgb_mod = cv2.cvtColor(hsv_mod, cv2.COLOR_HSV2RGB)

    # Обновление изображения на графике
    ax.imshow(rgb_mod)
    fig.canvas.draw_idle()

# Загрузка изображения
image_path = 'FRUITS.jpg'  # Укажи путь к изображению
image = cv2.cvtColor(cv2.imread(image_path), cv2.COLOR_BGR2RGB)

# Преобразование изображения в HSV
hsv_image = cv2.cvtColor(image, cv2.COLOR_RGB2HSV)

# Настройка окна графиков
fig, ax = plt.subplots(figsize=(8, 6))
plt.subplots_adjust(left=0.25, bottom=0.35)

# Отображение оригинального изображения
ax.imshow(image)
ax.set_title('Adjust HSV Parameters')

# Создание ползунков для изменения Hue, Saturation и Value
ax_h = plt.axes([0.25, 0.25, 0.65, 0.03], facecolor='lightgray')
ax_s = plt.axes([0.25, 0.18, 0.65, 0.03], facecolor='lightgray')
ax_v = plt.axes([0.25, 0.11, 0.65, 0.03], facecolor='lightgray')

# Ползунки для управления параметрами HSV
h_slider = Slider(ax_h, 'Hue', 0.5, 2.0, valinit=1.0)
s_slider = Slider(ax_s, 'Saturation', 0.5, 2.0, valinit=1.0)
v_slider = Slider(ax_v, 'Value', 0.5, 2.0, valinit=1.0)

# Обновление изображения при изменении ползунков
h_slider.on_changed(update)
s_slider.on_changed(update)
v_slider.on_changed(update)

# Кнопка для сохранения изображения с текущими настройками HSV
save_button = plt.axes([0.25, 0.02, 0.15, 0.04])
button = plt.Button(save_button, 'Save Image')

# Функция сохранения изображения
def save_image(event):
    # Получаем значения ползунков
    h = h_slider.val
    s = s_slider.val
    v = v_slider.val

    # Изменение изображения HSV по ползункам
    hsv_mod = hsv_image.copy()
    hsv_mod[:, :, 0] = (hsv_mod[:, :, 0].astype(np.float32) * h).astype(np.uint8) % 180
    hsv_mod[:, :, 1] = np.clip(hsv_mod[:, :, 1].astype(np.float32) * s, 0, 255).astype(np.uint8)
    hsv_mod[:, :, 2] = np.clip(hsv_mod[:, :, 2].astype(np.float32) * v, 0, 255).astype(np.uint8)

    # Преобразование обратно в RGB и сохранение изображения
    rgb_mod = cv2.cvtColor(hsv_mod, cv2.COLOR_HSV2RGB)
    cv2.imwrite('modified_image.jpg', cv2.cvtColor(rgb_mod, cv2.COLOR_RGB2BGR))
    print('Изображение сохранено как "modified_image.jpg"')

# Привязка функции сохранения к кнопке
button.on_clicked(save_image)

# Отображение графика с ползунками
plt.show()