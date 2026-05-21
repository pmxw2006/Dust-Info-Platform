import unittest
import importlib
import sys

# 确保能导入 DengLuCheSHI 包
sys.path.insert(0, r"C:\Users\gzyal\PyCharmMiscProject")

# 注意：没有 .py 后缀，用点号分隔！
MODULE_PATHS = [
    "DengLuCheSHI.ZhuCiTiXing",
    "DengLuCheSHI.DengLuShiYong",
    "DengLuCheSHI.wjmimashiy",
    "DengLuCheSHI.TianJiaTzi",
    "DengLuCheSHI.gerengzhongxinshanchuhexeigai",
    "DengLuCheSHI.gerenzhongxinguali",
    "DengLuCheSHI.liaotianshishyong",
    "DengLuCheSHI.kefu",
    "DengLuCheSHI.sixing",
    "DengLuCheSHI.tiziguanli",
    "DengLuCheSHI.zhanghufenjingshiyomng",
    "DengLuCheSHI.zhuyeichishi"
]
def collect_gz_tests():
    suite = unittest.TestSuite()
    loader = unittest.TestLoader()
    for module_name in MODULE_PATHS:
        try:
            module = importlib.import_module(module_name)
            for attr_name in dir(module):
                obj = getattr(module, attr_name)
                if (isinstance(obj, type) and
                    issubclass(obj, unittest.TestCase) and
                    obj.__name__ == "gz"):
                    suite.addTests(loader.loadTestsFromTestCase(obj))
        except Exception as e:
            print(f"警告：无法加载模块 {module_name}，原因：{e}")
    return suite

if __name__ == "__main__":
    runner = unittest.TextTestRunner(verbosity=2)
    test_suite = collect_gz_tests()
    runner.run(test_suite)