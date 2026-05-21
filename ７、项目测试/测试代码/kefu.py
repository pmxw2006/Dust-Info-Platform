import unittest
from DengLuCheSHI.kefugongju import Loginpage
from selenium import webdriver
from selenium.webdriver.chrome.options import Options
class gz(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        chrome_options = Options()
        # 设置一个非浏览器风格的 User-Agent，绕过 ngrok 警告页
        chrome_options.add_argument("--user-agent=MyTestRunner/1.0")
        cls.driver = webdriver.Chrome(options=chrome_options)
        cls.driver.maximize_window()
        cls.gz = Loginpage(cls.driver)
    @classmethod
    def tearDownClass(cls):
        cls.driver.quit()
    def setUp(self):
        self.driver = self.__class__.driver  # 重新绑定，覆盖掉之前可能的错误赋值
        self.driver.delete_all_cookies()
        self.gz.a1("https://aloft-rectify-salvaging.ngrok-free.dev/DengLu/DengLuYe")

    def test_cs1(self):
        #al整理正常使用
        self.gz.a2("10000005")
        self.gz.a3("123456")
        self.gz.a5()
        self.gz.a6()
        self.gz.a7("清理无用信息")
        self.gz.a8()
    def test_cs2(self):
        #al整理输入为空
        self.gz.a2("10000005")
        self.gz.a3("123456")
        self.gz.a5()
        self.gz.a6()
        self.gz.a7("")
        self.gz.a8()
    def test_cs3(self):
        #点击取消手动标记
        self.gz.a2("10000005")
        self.gz.a3("123456")
        self.gz.a5()
        self.gz.a6()
        self.gz.a9()
        self.gz.a10()
    def test_cs4(self):
        #点击手动标记
        self.gz.a2("10000005")
        self.gz.a3("123456")
        self.gz.a5()
        self.gz.a6()
        self.gz.a9()
        self.gz.a11()
    def test_cs5(self):
        #点击下一页
        self.gz.a2("10000005")
        self.gz.a3("123456")
        self.gz.a5()
        self.gz.a6()
        self.gz.a12()
    def test_cs6(self):
        #点击尾页
        self.gz.a2("10000005")
        self.gz.a3("123456")
        self.gz.a5()
        self.gz.a6()
        self.gz.a13()
    def test_cs7(self):
        #点击上一页
        self.gz.a2("10000005")
        self.gz.a3("123456")
        self.gz.a5()
        self.gz.a6()
        self.gz.a12()
        self.gz.a14()
    def test_cs8(self):
        #点击首页
        self.gz.a2("10000005")
        self.gz.a3("123456")
        self.gz.a5()
        self.gz.a6()
        self.gz.a12()
        self.gz.a15()



