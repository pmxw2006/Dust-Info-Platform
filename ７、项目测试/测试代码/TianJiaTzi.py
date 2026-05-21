import unittest
import time
import os
from DengLuCheSHI.TianJiaTZIGongfJu import Loginpage
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC


class gz(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        chrome_options = Options()
        chrome_options.add_argument("--user-agent=MyTestRunner/1.0")
        cls.driver = webdriver.Chrome(options=chrome_options)
        cls.driver.maximize_window()
        cls.gz = Loginpage(cls.driver)
        cls.wait = WebDriverWait(cls.driver, 10)

        # 测试图片路径（请修改为实际存在的图片）
        cls.test_image_path = r"C:\Users\gzyal\Desktop\8fc9d250d78b1d44d28b0487cbd4e007.jpg"
        if not os.path.exists(cls.test_image_path):
            print("⚠️ 警告：测试图片不存在，请修改 cls.test_image_path")

    @classmethod
    def tearDownClass(cls):
        cls.driver.quit()

    def setUp(self):
        self.driver = self.__class__.driver
        self.driver.delete_all_cookies()
        self.gz.a1("https://aloft-rectify-salvaging.ngrok-free.dev/DengLu/DengLuYe")
        self.gz.a2("10000010")
        self.gz.a3("123456")
        self.gz.a5()
        time.sleep(2)

    # ---------- 辅助方法 ----------
    def get_file_input(self):
        try:
            return self.driver.find_element(By.ID, "hidden-file-input")
        except:
            return self.driver.find_element(By.CSS_SELECTOR, "input[type='file']")

    def input_title(self, title):
        el = self.wait.until(EC.element_to_be_clickable(self.gz._biaoti))
        el.clear()
        el.send_keys(title)

    def input_content(self, content):
        el = self.wait.until(EC.element_to_be_clickable(self.gz._tzineir))
        el.clear()
        el.send_keys(content)

    def get_title_error(self):
        try:
            el = self.wait.until(EC.visibility_of_element_located((By.CSS_SELECTOR, "#tianjia-app > form > div:nth-child(1) > span > span")))
            text = el.text.strip()
            return text
        except:
            return ""

    def get_content_error(self):
        try:
            el = self.wait.until(EC.visibility_of_element_located((By.CSS_SELECTOR, "#tianjia-app > form > div:nth-child(3) > span > span")))
            text = el.text.strip()
            return text
        except:
            return ""

    # ==================== 测试用例 ====================
    def test_01_normal_add_post_with_image(self):
        """正常添加帖子（含一张图片）- 无断言，仅执行操作"""
        self.gz.tianjiatzi()
        time.sleep(1)

        self.input_title("自动化测试帖子_带图片")
        self.input_content("这是通过Selenium发布的帖子内容，包含一张测试图片。")

        file_input = self.get_file_input()
        file_input.send_keys(self.test_image_path)
        time.sleep(1)

        self.gz.fabu()
        time.sleep(2)   # 等待操作完成
        print("✅ 正常添加帖子操作完成（无验证）")

    def test_02_title_empty(self):
        """标题为空：应显示错误提示"""
        self.gz.tianjiatzi()
        time.sleep(1)

        self.input_title("")
        self.input_content("内容正常")
        self.gz.fabu()
        time.sleep(0.5)

        error_text = self.get_title_error()
        self.assertNotEqual(error_text, "", "标题为空时无错误提示")
        self.assertIn("不能为空", error_text)
        print("✅ 标题为空测试通过")

    def test_03_title_max_length(self):
        """标题超长：有限制则返回错误，无限制则成功 - 无断言"""
        self.gz.tianjiatzi()
        time.sleep(1)

        long_title = "超长标题" * 50
        self.input_title(long_title)
        self.input_content("内容正常")
        self.gz.fabu()
        time.sleep(1)
        print("✅ 标题超长测试操作完成（无验证）")

    def test_04_content_empty(self):
        """内容为空：应显示错误提示"""
        self.gz.tianjiatzi()
        time.sleep(1)

        self.input_title("标题正常")
        self.input_content("")
        self.gz.fabu()
        time.sleep(0.5)

        error_text = self.get_content_error()
        self.assertNotEqual(error_text, "", "内容为空时无错误提示")
        self.assertIn("不能为空", error_text)
        print("✅ 内容为空测试通过")

    def test_05_content_max_length(self):
        """内容超长测试（无断言，仅执行操作）"""
        self.gz.tianjiatzi()
        time.sleep(1)

        long_content = "测" * 1001
        self.input_title("标题正常")
        self.input_content(long_content)
        self.gz.fabu()
        time.sleep(1)
        print("✅ 内容超长测试执行完成（无断言）")

    def test_06_image_limit_9_and_boundary_10(self):
        """图片最多9张，尝试第10张应被阻止"""
        self.gz.tianjiatzi()
        time.sleep(1)
        self.input_title("图片上限测试")
        self.input_content("测试内容")

        file_input = self.get_file_input()
        for _ in range(9):
            file_input.send_keys(self.test_image_path)
            time.sleep(0.2)

        time.sleep(1)
        previews = self.driver.find_elements(By.CSS_SELECTOR, ".yulan-kuang")
        self.assertLessEqual(len(previews), 9, f"预览图数量{len(previews)}超过9张")

        # 尝试第10张
        file_input.send_keys(self.test_image_path)
        time.sleep(0.5)
        new_previews = self.driver.find_elements(By.CSS_SELECTOR, ".yulan-kuang")
        self.assertLessEqual(len(new_previews), 9, "上传第10张后预览图数量增加，上限未生效")

        try:
            alert = self.driver.switch_to.alert
            alert.accept()
        except:
            pass

        print("✅ 图片上限测试通过")
    def test_07(self):
        self.gz.tianjiatzi()
        self.gz.fanhuishangyy()


if __name__ == "__main__":
    unittest.main()