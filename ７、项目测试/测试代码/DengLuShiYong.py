import unittest
from DengLuCheSHI.DengLuFongZhuang import Loginpage
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
        #账号，密码,正确
        self.gz.a2("10000010")
        self.gz.a3("123456")
        self.gz.a5()
        self.assertTrue(self.gz.a7(),"断言失败未能跳转到后台")
    def test_cs2(self):
        #账号正确，密码错误
        self.gz.a2("10000010")
        self.gz.a3("1234")
        self.gz.a5()
        self.assertIn("密码",self.gz.a6())
    def test_cs3(self):
        #账号错误，密码正确
        self.gz.a2("12345")
        self.gz.a3("123456")
        self.gz.a5()
        self.assertIn("账号", self.gz.a6())
    def test_cs4(self):
        #账号为空，密码正确
        self.gz.a2("")
        self.gz.a3("123456")
        self.gz.a5()
        self.assertIn("账户", self.gz.a8())
    def test_cs5(self):
        #账号正确，密码为空
        self.gz.a2("10000010")
        self.gz.a3("")
        self.gz.a5()
        self.assertIn("密码", self.gz.a9())
    def test_cs6(self):
        #一位数的账户和正确的密码
        self.gz.a2("8")
        self.gz.a3("123456")
        self.gz.a5()
        self.assertIn("账号或密码错误", self.gz.a6())
    def test_cs7(self):
        # 21位数的账户和正确的密码
        self.gz.a2("1" * 21)  # 生成 20 个 "1"
        self.gz.a3("123456")
        self.gz.a5()
        self.assertIn("账号或密码错误", self.gz.a6())
    def test_cs8(self):
        # 带特殊字符的账户和正确的密码
        self.gz.a2("%&*^&%**&%^&%%#%^*HGTfsudgys")
        self.gz.a3("123456")
        self.gz.a5()
        self.assertIn("账号或密码错误", self.gz.a6())
    def test_cs9(self):
        # 正确的账户，一位数的密码
        self.gz.a2("10000010")
        self.gz.a3("1")
        self.gz.a5()
        self.assertIn("账号或密码错误", self.gz.a6())
    def test_cs10(self):
        # 正确的账户，21位数的密码
        self.gz.a2("10000010")
        self.gz.a3("1" * 21)
        self.gz.a5()
        self.assertIn("账号或密码错误", self.gz.a6())
    def test_cs11(self):
         # 正确的账户，带特殊字符的密码
        self.gz.a2("10000010")
        self.gz.a3("%&*^&%**&%^&%%#%^*HGTfsudgys")
        self.gz.a5()
        self.assertIn("账号或密码错误", self.gz.a6())
    def test_cs12(self):
        # 正确的账户，带特殊字符的密码
        self.gz.a2("10000010")
        self.gz.a3("%&*^&%**&%^&%%#%^*HGTfsudgys")
        self.gz.a5()
        self.assertIn("账号或密码错误", self.gz.a6())
    def test_cs13(self):
        # 账号，密码,正确,选择记住密码
        self.gz.a2("10000010")
        self.gz.a3("123456")
        self.gz.a10()
        self.gz.a5()
        self.assertTrue(self.gz.a7(), "断言失败未能跳转到后台")
    def test_cs14(self):
        # 账号正确，密码错误,选择记住密码
        self.gz.a2("10000010")
        self.gz.a3("1234")
        self.gz.a10()
        self.gz.a5()
        self.assertIn("密码", self.gz.a6())

    def test_cs15(self):
        # 账号错误，密码正确,选择记住密码
        self.gz.a2("12345")
        self.gz.a3("123456")
        self.gz.a10()
        self.gz.a5()
        self.assertIn("账号", self.gz.a6())

    def test_cs16(self):
        # 账号为空，密码正确,选择记住密码
        self.gz.a2("")
        self.gz.a3("123456")
        self.gz.a10()
        self.gz.a5()
        self.assertIn("账户", self.gz.a8())

    def test_cs17(self):
        # 账号正确，密码为空,选择记住密码
        self.gz.a2("10000010")
        self.gz.a3("")
        self.gz.a10()
        self.gz.a5()
        self.assertIn("密码", self.gz.a9())

    def test_cs18(self):
        # 一位数的账户和正确的密码,选择记住密码
        self.gz.a2("8")
        self.gz.a3("123456")
        self.gz.a10()
        self.gz.a5()
        self.assertIn("账号或密码错误", self.gz.a6())

    def test_cs19(self):
        # 21位数的账户和正确的密码,选择记住密码
        self.gz.a2("1" * 21)  # 生成 20 个 "1"
        self.gz.a3("123456")
        self.gz.a10()
        self.gz.a5()
        self.assertIn("账号或密码错误", self.gz.a6())

    def test_cs20(self):
        # 带特殊字符的账户和正确的密码,选择记住密码
        self.gz.a2("%&*^&%**&%^&%%#%^*HGTfsudgys")
        self.gz.a3("123456")
        self.gz.a10()
        self.gz.a5()
        self.assertIn("账号或密码错误", self.gz.a6())

    def test_cs21(self):
        # 正确的账户，一位数的密码,选择记住密码
        self.gz.a2("10000010")
        self.gz.a3("1")
        self.gz.a10()
        self.gz.a5()
        self.assertIn("账号或密码错误", self.gz.a6())

    def test_cs22(self):
        # 正确的账户，21位数的密码,选择记住密码
        self.gz.a2("10000010")
        self.gz.a3("1" * 21)
        self.gz.a10()
        self.gz.a5()
        self.assertIn("账号或密码错误", self.gz.a6())

    def test_cs23(self):
        # 正确的账户，带特殊字符的密码,选择记住密码
        self.gz.a2("10000010")
        self.gz.a3("%&*^&%**&%^&%%#%^*HGTfsudgys")
        self.gz.a10()
        self.gz.a5()
        self.assertIn("账号或密码错误", self.gz.a6())

    def test_cs24(self):
        # 正确的账户，带特殊字符的密码,选择记住密码
        self.gz.a2("10000010")
        self.gz.a3("%&*^&%**&%^&%%#%^*HGTfsudgys")
        self.gz.a10()
        self.gz.a5()
        self.assertIn("账号或密码错误", self.gz.a6())












