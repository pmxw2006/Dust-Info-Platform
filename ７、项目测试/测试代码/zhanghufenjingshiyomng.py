import unittest
from selenium import webdriver
from selenium.webdriver.chrome.options import Options
from DengLuCheSHI.zhanghufengzhang import Loginpage   # 假设文件名，或者直接在此文件中定义

class AccountBanTest(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        chrome_options = Options()
        chrome_options.add_argument("--user-agent=MyTestRunner/1.0")
        cls.driver = webdriver.Chrome(options=chrome_options)
        cls.driver.maximize_window()
        cls.pc = Loginpage(cls.driver)
    @classmethod
    def tearDownClass(cls):
        cls.driver.quit()
    def setUp(self):
        self.driver = self.__class__.driver  # 重新绑定，覆盖掉之前可能的错误赋值
        self.driver.delete_all_cookies()
        self.pc.a1("https://aloft-rectify-salvaging.ngrok-free.dev/DengLu/DengLuYe")
    def test_cs1(self):
        #确定封禁
        self.pc.a2("10000005")
        self.pc.a3("123456")
        self.pc.a5()
        self.pc.a6()  # 点击账户管理
        self.pc.a7()  # 点击封禁
        self.pc.a8()  # 确认封禁
    def test_cs2(self):
        # 确定解封
        self.pc.a2("10000005")
        self.pc.a3("123456")
        self.pc.a5()
        self.pc.a6()  # 点击账户管理
        self.pc.a7()  # 点击封禁
        self.pc.a8()  # 确认封禁
    def test_cs3(self):
        # 取消
        self.pc.a2("10000005")
        self.pc.a3("123456")
        self.pc.a5()
        self.pc.a6()  # 点击账户管理
        self.pc.a7()  # 点击封禁
        self.pc.a9()  # 取消

    def test_cs4(self):
        # 取消
        self.pc.a2("10000005")
        self.pc.a3("123456")
        self.pc.a5()
        self.pc.a6()  # 点击账户管理
        self.pc.a7()  # 点击封禁
        self.pc.a10("123")
        self.pc.a11("123")
        self.pc.a12("123")
        self.pc.a13()
    def test_cs5(self):
        # 下一页
        self.pc.a2("10000005")
        self.pc.a3("123456")
        self.pc.a5()
        self.pc.a6()  # 点击账户管理
        self.pc.a17()

    def test_cs6(self):
        # 尾页
        self.pc.a2("10000005")
        self.pc.a3("123456")
        self.pc.a5()
        self.pc.a6()  # 点击账户管理
        self.pc.a16()
    def test_cs7(self):
        # 上一页
        self.pc.a2("10000005")
        self.pc.a3("123456")
        self.pc.a5()
        self.pc.a6()  # 点击账户管理
        self.pc.a17()
        self.pc.a15()
    def test_cs8(self):
        # 首页
        self.pc.a2("10000005")
        self.pc.a3("123456")
        self.pc.a5()
        self.pc.a6()  # 点击账户管理
        self.pc.a17()
        self.pc.a14()
    def test_cs9(self):
        # 搜索用户名
        self.pc.a2("10000005")
        self.pc.a3("123456")
        self.pc.a5()
        self.pc.a6()  # 点击账户管理
        self.pc.a10("天下为公")
        self.pc.a18()
    def test_cs10(self):
        # 搜索邮箱
        self.pc.a2("10000005")
        self.pc.a3("123456")
        self.pc.a5()
        self.pc.a6()  # 点击账户管理
        self.pc.a11("new@test.com")
        self.pc.a18()
    def test_cs11(self):
        # 搜索用户id
        self.pc.a2("10000005")
        self.pc.a3("123456")
        self.pc.a5()
        self.pc.a6()  # 点击账户管理
        self.pc.a12("10000009")
        self.pc.a18()



if __name__ == '__main__':
    unittest.main()