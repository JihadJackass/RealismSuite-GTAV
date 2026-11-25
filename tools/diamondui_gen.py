# Artwork for the Diamond skin: cards, card back, chips, frame ornaments. Original designs (no game or brand art).
from PIL import Image, ImageDraw, ImageFont, ImageFilter
import math, os
OUT = os.path.join(os.path.dirname(__file__), 'out')
SANS = '/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf'
SERIF_B = '/usr/share/fonts/truetype/dejavu/DejaVuSerif-Bold.ttf'
GOLD = (214, 178, 92); GOLD_D = (150, 118, 52); IVORY = (248, 244, 234); INK = (24, 22, 26); RED = (176, 26, 34)
S = 2  # draw at twice the size, then scale down for smooth edges

def font(p, n): return ImageFont.truetype(p, n * S)

def card(rank, suit):
    W, H = 140, 196
    im = Image.new('RGBA', (W * S, H * S), (0, 0, 0, 0)); d = ImageDraw.Draw(im)
    d.rounded_rectangle([0, 0, W * S - 1, H * S - 1], 12 * S, fill=IVORY, outline=(190, 180, 160), width=S)
    d.rounded_rectangle([6 * S, 6 * S, (W - 6) * S, (H - 6) * S], 8 * S, outline=GOLD, width=S)
    col = RED if suit in 'hd' else INK
    glyph = {'s': '♠', 'h': '♥', 'd': '♦', 'c': '♣'}[suit]
    fi, fs = font(SERIF_B, 22), font(SANS, 18)
    def corner(img):
        dd = ImageDraw.Draw(img)
        dd.text((13 * S, 9 * S), rank, font=fi, fill=col)
        dd.text((13 * S, 34 * S), glyph, font=fs, fill=col)
    corner(im)
    im2 = im.rotate(180); corner(im2); im = im2.rotate(180)
    d = ImageDraw.Draw(im)
    cx, cy = W * S / 2, H * S / 2
    if rank in ('J', 'Q', 'K'):
        # a framed panel with the letter large in gold, the suit above and below
        d.rectangle([34 * S, 38 * S, (W - 34) * S, (H - 38) * S], fill=(32, 28, 30), outline=GOLD, width=2 * S)
        d.rectangle([39 * S, 43 * S, (W - 39) * S, (H - 43) * S], outline=GOLD_D, width=S)
        fl = font(SERIF_B, 54); tw = d.textlength(rank, font=fl)
        d.text((cx - tw / 2, cy - 38 * S), rank, font=fl, fill=GOLD)
        fg = font(SANS, 20)
        for yy in (50, H - 72):
            gw = d.textlength(glyph, font=fg); d.text((cx - gw / 2, yy * S), glyph, font=fg, fill=(230, 60, 60) if suit in 'hd' else IVORY)
    elif rank == 'A':
        fg = font(SANS, 84 if suit == 's' else 70); gw = d.textlength(glyph, font=fg)
        d.text((cx - gw / 2, cy - 58 * S), glyph, font=fg, fill=col)
        if suit == 's': d.ellipse([cx - 44 * S, cy - 44 * S, cx + 44 * S, cy + 44 * S], outline=GOLD, width=S)
    else:
        n = int(rank)
        L = {2: [(0, -1), (0, 1)], 3: [(0, -1), (0, 0), (0, 1)], 4: [(-1, -1), (1, -1), (-1, 1), (1, 1)],
             5: [(-1, -1), (1, -1), (0, 0), (-1, 1), (1, 1)], 6: [(-1, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (1, 1)],
             7: [(-1, -1), (1, -1), (0, -.5), (-1, 0), (1, 0), (-1, 1), (1, 1)],
             8: [(-1, -1), (1, -1), (0, -.5), (-1, 0), (1, 0), (0, .5), (-1, 1), (1, 1)],
             9: [(-1, -1), (1, -1), (-1, -1 / 3), (1, -1 / 3), (0, 0), (-1, 1 / 3), (1, 1 / 3), (-1, 1), (1, 1)],
             10: [(-1, -1), (1, -1), (0, -2 / 3), (-1, -1 / 3), (1, -1 / 3), (-1, 1 / 3), (1, 1 / 3), (0, 2 / 3), (-1, 1), (1, 1)]}[n]
        fg = font(SANS, 28)
        for px, py in L:
            x = cx + px * 25 * S; y = cy + py * 50 * S
            g = Image.new('RGBA', (40 * S, 40 * S), (0, 0, 0, 0))
            ImageDraw.Draw(g).text((20 * S, 20 * S), glyph, font=fg, fill=col, anchor='mm')
            if py > 0.1: g = g.rotate(180)
            im.alpha_composite(g, (int(x - 20 * S), int(y - 20 * S)))
    return im.resize((W, H), Image.LANCZOS)

def back():
    W, H = 140, 196
    im = Image.new('RGBA', (W * S, H * S), (0, 0, 0, 0)); d = ImageDraw.Draw(im)
    d.rounded_rectangle([0, 0, W * S - 1, H * S - 1], 12 * S, fill=(14, 12, 14), outline=GOLD_D, width=S)
    d.rounded_rectangle([8 * S, 8 * S, (W - 8) * S, (H - 8) * S], 7 * S, outline=GOLD, width=2 * S)
    step = 14 * S
    for i in range(-20, 30):   # a lattice of diamonds
        x0 = 8 * S + i * step
        d.line([(x0, 8 * S), (x0 + (H - 16) * S, (H - 8) * S)], fill=(70, 58, 30), width=S)
        d.line([(x0, (H - 8) * S), (x0 + (H - 16) * S, 8 * S)], fill=(70, 58, 30), width=S)
    mask = Image.new('L', im.size, 0); ImageDraw.Draw(mask).rounded_rectangle([10 * S, 10 * S, (W - 10) * S, (H - 10) * S], 6 * S, fill=255)
    base = Image.new('RGBA', im.size, (0, 0, 0, 0)); bd = ImageDraw.Draw(base)
    bd.rounded_rectangle([0, 0, W * S - 1, H * S - 1], 12 * S, fill=(14, 12, 14), outline=GOLD_D, width=S)
    base.paste(im, (0, 0), mask); d = ImageDraw.Draw(base)
    d.rounded_rectangle([8 * S, 8 * S, (W - 8) * S, (H - 8) * S], 7 * S, outline=GOLD, width=2 * S)
    cx, cy = W * S / 2, H * S / 2; r = 30 * S
    d.polygon([(cx, cy - r * 1.3), (cx + r, cy), (cx, cy + r * 1.3), (cx - r, cy)], fill=(14, 12, 14), outline=GOLD)
    r2 = 18 * S
    d.polygon([(cx, cy - r2 * 1.3), (cx + r2, cy), (cx, cy + r2 * 1.3), (cx - r2, cy)], fill=GOLD)
    return base.resize((W, H), Image.LANCZOS)

def chip(name, body, stripe, label):
    R = 64
    im = Image.new('RGBA', (R * 2 * S, R * 2 * S), (0, 0, 0, 0)); d = ImageDraw.Draw(im)
    c = R * S
    d.ellipse([2 * S, 2 * S, (2 * R - 2) * S, (2 * R - 2) * S], fill=body)
    for k in range(8):   # edge spots
        a0 = k * 45 - 9; a1 = k * 45 + 9
        d.pieslice([2 * S, 2 * S, (2 * R - 2) * S, (2 * R - 2) * S], a0, a1, fill=stripe)
    d.ellipse([16 * S, 16 * S, (2 * R - 16) * S, (2 * R - 16) * S], fill=body)
    d.ellipse([20 * S, 20 * S, (2 * R - 20) * S, (2 * R - 20) * S], outline=GOLD, width=2 * S)
    d.ellipse([26 * S, 26 * S, (2 * R - 26) * S, (2 * R - 26) * S], fill=(20, 18, 20))
    f = font(SERIF_B, 26 if len(label) <= 3 else 20); tw = d.textlength(label, font=f)
    d.text((c - tw / 2, c - 17 * S), label, font=f, fill=GOLD)
    d.ellipse([2 * S, 2 * S, (2 * R - 2) * S, (2 * R - 2) * S], outline=(0, 0, 0, 120), width=S)
    im = im.resize((R * 2, R * 2), Image.LANCZOS); im.save(os.path.join(OUT, 'chip_' + name + '.png'))

def deco():
    # a corner: stepped gold lines and a small diamond, for the top-left; the kit mirrors it for the others
    N = 48
    im = Image.new('RGBA', (N * S, N * S), (0, 0, 0, 0)); d = ImageDraw.Draw(im)
    for k, a in enumerate((255, 200, 140)):
        o = (2 + k * 6) * S; L = (N - 4 - k * 10) * S
        d.line([(o, o + L), (o, o), (o + L, o)], fill=GOLD + (a,), width=2 * S if k == 0 else S)
    cx = cy = 16 * S; r = 5 * S
    d.polygon([(cx, cy - r), (cx + r, cy), (cx, cy + r), (cx - r, cy)], fill=GOLD)
    im.resize((N, N), Image.LANCZOS).save(os.path.join(OUT, 'deco_corner.png'))
    W, H = 512, 16
    im = Image.new('RGBA', (W * S, H * S), (0, 0, 0, 0)); d = ImageDraw.Draw(im)
    for x in range(W * S):
        a = int(255 * min(1, (1 - abs(x / (W * S) - 0.5) * 2) * 1.6))
        d.line([(x, H * S / 2), (x, H * S / 2 + S)], fill=GOLD + (a,))
    cx, cy = W * S / 2, H * S / 2 + S / 2; r = 6 * S
    d.polygon([(cx, cy - r), (cx + r * 1.4, cy), (cx, cy + r), (cx - r * 1.4, cy)], fill=GOLD)
    im.resize((W, H), Image.LANCZOS).save(os.path.join(OUT, 'deco_rule.png'))

os.makedirs(OUT, exist_ok=True)
for s in 'shdc':
    for r in ['A', '2', '3', '4', '5', '6', '7', '8', '9', '10', 'J', 'Q', 'K']:
        card(r, s).save(os.path.join(OUT, 'card_%s%s.png' % (r, s)))
back().save(os.path.join(OUT, 'card_back.png'))
chip('100', (236, 232, 222), (40, 90, 170), '100')
chip('500', (110, 40, 150), (236, 232, 222), '500')
chip('1k', (26, 24, 28), (214, 178, 92), '1K')
chip('5k', (170, 28, 36), (236, 232, 222), '5K')
chip('25k', (40, 120, 80), (236, 232, 222), '25K')
chip('100k', (214, 178, 92), (26, 24, 28), '100K')
deco()
print(len(os.listdir(OUT)), 'files')
c = Image.open(os.path.join(OUT, 'deco_corner.png'))
c.save(os.path.join(OUT, 'deco_corner_tl.png'))
c.transpose(Image.FLIP_LEFT_RIGHT).save(os.path.join(OUT, 'deco_corner_tr.png'))
c.transpose(Image.FLIP_TOP_BOTTOM).save(os.path.join(OUT, 'deco_corner_bl.png'))
c.transpose(Image.ROTATE_180).save(os.path.join(OUT, 'deco_corner_br.png'))
os.remove(os.path.join(OUT, 'deco_corner.png'))
print(sorted(os.listdir(OUT))[-8:])
