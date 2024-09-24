import numpy as np
from PIL import Image
import matplotlib.pyplot as plt

# Функция для преобразования изображения в оттенки серого
def rgb_to_gray(image, coefficients):
    # Преобразуем изображение в numpy-массив
    image_np = np.array(image)
    # Применяем формулу: Y = coef[0]*R + coef[1]*G + coef[2]*B
    gray_image = (coefficients[0] * image_np[:,:,0] +
                  coefficients[1] * image_np[:,:,1] +
                  coefficients[2] * image_np[:,:,2]).astype(np.float32)
    return gray_image

# Функция для вычисления нормированной разности двух изображений
def calculate_difference(image1, image2):
    # Избегаем отрицательных значений, нормируем на максимум для относительной разности
    diff = np.abs(image1 - image2)
    max_diff = np.max(diff)
    if max_diff > 0:
        norm_diff = diff / max_diff  # Нормируем на максимальную разность
    else:
        norm_diff = diff  # Если нет разности, оставляем как есть
    return (norm_diff * 255).astype(np.uint8)  # Масштабируем в диапазон от 0 до 255

# Загрузка изображения
image = Image.open('FRUITS.jpg')

# Формула 1: Y = 0.299R + 0.587G + 0.114B
coefficients1 = [0.299, 0.587, 0.114]
gray_image1 = rgb_to_gray(image, coefficients1)

# Формула 2: Y = 0.2126R + 0.7152G + 0.0722B
coefficients2 = [0.2126, 0.7152, 0.0722]
gray_image2 = rgb_to_gray(image, coefficients2)

# Находим нормированную разность между двумя изображениями
difference_image = calculate_difference(gray_image1, gray_image2)

# Построение гистограмм и изображений
plt.figure(figsize=(12, 6))

# Гистограмма для первого изображения
plt.subplot(2, 3, 1)
plt.hist(gray_image1.ravel(), bins=256, range=(0, 256), color='gray')
plt.title('Histogram (Formula 1)')

# Гистограмма для второго изображения
plt.subplot(2, 3, 2)
plt.hist(gray_image2.ravel(), bins=256, range=(0, 256), color='gray')
plt.title('Histogram (Formula 2)')

# Гистограмма для разности изображений
plt.subplot(2, 3, 3)
plt.hist(difference_image.ravel(), bins=256, range=(0, 256), color='gray')
plt.title('Histogram (Difference)')

# Отображение первого изображения в оттенках серого
plt.subplot(2, 3, 4)
plt.imshow(gray_image1, cmap='gray')
plt.title('Grayscale Image (Formula 1)')

# Отображение второго изображения в оттенках серого
plt.subplot(2, 3, 5)
plt.imshow(gray_image2, cmap='gray')
plt.title('Grayscale Image (Formula 2)')

# Отображение разности изображений
plt.subplot(2, 3, 6)
plt.imshow(difference_image, cmap='gray')
plt.title('Difference Image')

# Отображение всех результатов
plt.tight_layout()
plt.show()

# Сохранение изображений
Image.fromarray(gray_image1.astype(np.uint8)).save('gray_image1.jpg')
Image.fromarray(gray_image2.astype(np.uint8)).save('gray_image2.jpg')
Image.fromarray(difference_image).save('difference_image.jpg')
